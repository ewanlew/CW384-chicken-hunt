[System.Serializable]
public class ScoreEntry {
    public int score;
    public string date;
    public string time;

    public ScoreEntry(int score) {
        this.score = score;
        this.date = System.DateTime.Now.ToString("dd/MM/yyyy");
        this.time = System.DateTime.Now.ToString("HH:mm:ss");
    }
}
