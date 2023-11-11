/* Function: manages the read and write of saved data files
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 02/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Photon.Pun;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    private bool useEncryption = false;

    private readonly string encryptionCodeWord = "TNTeam";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        //Path.Combine for different OS's
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load the serialized data from json
                string dataToLoad = "";
                //Read data from file
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //If useEncryption is enable: decrypt data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                //deserialize data from json into C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                if (PhotonNetwork.InRoom) DataPersistenceManager.instance.GetComponent<PhotonView>().RPC("LoadClientData", RpcTarget.Others, dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error al cargar: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data){
        //Path.Combine for different OS's
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //Creates the directory where the file will be saved
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //Serialize game data to json
            string dataToStore = JsonUtility.ToJson(data, true);
            //If useEncryption is enable: encrypt data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            //Write data to file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar: " + fullPath + "\n" + e);
        }
    }

    public bool FileValidation()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        bool isFileCreated = File.Exists(fullPath);
        return isFileCreated;
    }

    //Encrypts and decrypts the saved data to prevent players from changing values on text editor
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
