using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    private DialogueSystem dialogue;
    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueSystem.instance;
    }
    
    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogue.isSpeaking || dialogue.isWaitingForUserInput)
            {
                if (index >= script.Length)
                {
                    return;
                }
                Say(script[index]);
                index++;
            }
        }
        
    }

    void Say(string s)
    {
        string[] parts = s.Split('*');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";
        string background = (parts.Length >= 3) ? parts[2] : "";
        string chara1 = (parts.Length >= 4) ? parts[3] : "";
        string chara2 = (parts.Length >= 5) ? parts[4] : "";

        dialogue.Say(speech, speaker);
    }
    
    public string[] script = new string[]
    {
        "And with that, a piercing scream cuts through the frigid night.*Narrator",
        "YIKES! That was a scary dream. It's like the screaming was real. I'm glad I was just asleep. I wonder how much time I have until work…***Park_Day_BG*Capy_Normal_v02",
        "8:51?! OH NO! Forget what I said earlier! I'd rather someone be dead than be late to work today!",
        "Shoving a handful of Grassy-O's into my mouth, I grab my hat and coat and run out the door.",
        "In my haste, I don't chew my Grassy-O's fast enough. They fall out of my mouth just as I reach the steps outside.",
        "Whoah!*Detective Capybara",
        "Th-the steps! They're so icy!*Narrator"
    };
}
