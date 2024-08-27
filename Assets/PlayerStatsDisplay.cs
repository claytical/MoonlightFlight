using UnityEngine;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI itemsCollectedText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI fuelUsedText;
    public TextMeshProUGUI comparisonLabel;

    public void PopulateStats(PlayerStatsTracking stats, PlayerStatsTracking[] allPlayers)
    {
        // Populate the individual stats
        distanceText.text = $"Distance Traveled: {stats.distanceCovered:F2}m";
        itemsCollectedText.text = $"Items Collected: {stats.itemsCollected}";
        scoreText.text = $"Assists: {stats.effiencyScore}";
        fuelUsedText.text = $"Fuel Used: {stats.fuelUsed}";

        // Generate a funny comparison label
        comparisonLabel.text = GenerateFunnyLabel(stats, allPlayers);
    }

    private string GenerateFunnyLabel(PlayerStatsTracking stats, PlayerStatsTracking[] allPlayers)
    {
        // Comparison logic to determine who is best at what
        int bestDistance = 0, mostItems = 0, mostAssists = 0, leastFuel = 0;

        foreach (var player in allPlayers)
        {
            if (player.distanceCovered > allPlayers[bestDistance].distanceCovered)
                bestDistance = System.Array.IndexOf(allPlayers, player);
            if (player.itemsCollected > allPlayers[mostItems].itemsCollected)
                mostItems = System.Array.IndexOf(allPlayers, player);
            if (player.assists > allPlayers[mostAssists].assists)
                mostAssists = System.Array.IndexOf(allPlayers, player);
            if (player.fuelUsed < allPlayers[leastFuel].fuelUsed)
                leastFuel = System.Array.IndexOf(allPlayers, player);
        }

        string funnyName = "The ";

        if (stats == allPlayers[bestDistance])
            funnyName += "Speedster";
        else if (stats == allPlayers[mostItems])
            funnyName += "Collector";
        else if (stats == allPlayers[mostAssists])
            funnyName += "Support Hero";
        else if (stats == allPlayers[leastFuel])
            funnyName += "Eco Warrior";
        else
            funnyName += "Jack of All Trades";

        return funnyName;
    }
}
