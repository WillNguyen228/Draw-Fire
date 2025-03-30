using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class combatManaging : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    static int flychoice;
    int SOMMERSAULT = 0;
    int REPRODUCE = 1;
    int MUTATE = 2;

    static int humanchoice;
    int NEWSPAPER = 0;
    int TOXIN = 1;
    int MAGIC = 2;

    DepletesHealth FlyHealth;
    DepletesHealth HumanHealth;
    TextMeshProUGUI descriptiontext;

    void Start() {
        flychoice = UnityEngine.Random.Range(0, 2);
        FlyHealth = GameObject.Find("barbackgroundmosquito/fill").GetComponent<DepletesHealth>();
        HumanHealth = GameObject.Find("barbackgroundhuman/fill").GetComponent<DepletesHealth>();
        descriptiontext = GameObject.Find("description").GetComponent<TextMeshProUGUI>();
    }

    public void SetPlayerChoiceToNewspaper() {
        humanchoice = 0;
        descriptiontext.text = "The human chooses to fight with the newspaper!";
    }

    public void DetermineOutcome() {
        if (flychoice == SOMMERSAULT) {
            descriptiontext.text += " And the mosquitos chose to charge at the human!";
            if (humanchoice == NEWSPAPER) {
                FlyHealth.ReduceHealth(2);
                descriptiontext.text += " Their sommersaults are no match for the human's newspaper skills";
            } else if (humanchoice == TOXIN) {
                HumanHealth.ReduceHealth(2);
                descriptiontext.text += " Their constant buzzing make you drop the toxin on the floor";
            } else {
                FlyHealth.ReduceHealth(1);
                HumanHealth.ReduceHealth(1);
                descriptiontext.text += " Both sides annoy each other with frantic gestures";
            }
        } else if (flychoice == REPRODUCE) {
            descriptiontext.text += " And the mosquitos chose to reproduce!";
            if (humanchoice == NEWSPAPER) {
                FlyHealth.ReduceHealth(1);
                HumanHealth.ReduceHealth(1);
                descriptiontext.text += " Some mosquitos are swatted, but they still keep coming";
            } else if (humanchoice == TOXIN) {
                FlyHealth.ReduceHealth(2);
                descriptiontext.text += " All their babies are dead!";
            } else {
                HumanHealth.ReduceHealth(2);
                descriptiontext.text += " Their babies learned magic from the humans!";
            }
        } else {
            descriptiontext.text += " And the mosquitos chose to send their mutants!";
            if (humanchoice == NEWSPAPER) {
                HumanHealth.ReduceHealth(2);
                descriptiontext.text += " The newspaper gets torn apart by the mutant strength!";
            } else if (humanchoice == TOXIN) {
                HumanHealth.ReduceHealth(1);
                FlyHealth.ReduceHealth(1);
                descriptiontext.text += " The mutants survive, but were distraught by the babies that were killed!";
            } else {
                FlyHealth.ReduceHealth(2);
                descriptiontext.text += " Turns out the mutant are allergic to magic!";
            }
        }
    }

    public void SetPlayerChoiceToToxin() {
        humanchoice = 1;
        descriptiontext.text = "The human chooses to fight with the toxin!";
    }

    public void SetPlayerChoiceToMagic() {
        humanchoice = 2;
        descriptiontext.text = "The human chooses to fight with magic!";
    }

    public void FlyChoose() {
        flychoice = UnityEngine.Random.Range(0, 3);
    }
    
}
