using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length;)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();

            dialogue.name = row[1];
            List<string> contextList = new List<string>();
            //List<string> selectList = new List<string>();
            //List<string> skipList = new List<string>();
            //List<string> imageList = new List<string>();
            //List<string> eventList = new List<string>();

            do
            {
                contextList.Add(row[2]);
                //selectList.Add(row[3]);
                //skipList.Add(row[4]);
                //imageList.Add(row[5]);
                //eventList.Add(row[6]);

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }

            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();
            //dialogue.choicenum = selectList.ToArray();
            //dialogue.skipnum = skipList.ToArray();
            //dialogue.imgname = imageList.ToArray();
            //dialogue.events = eventList.ToArray();
            dialogueList.Add(dialogue);

        }
        return dialogueList.ToArray();
    }
}
