#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using WheelOfFortune.Debug;

namespace WheelOfFortune.EditorTools
{
    [CustomEditor(typeof(DebugPanel))]
    public class DebugPanelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space(10);

            DebugPanel panel = (DebugPanel)target;

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    "Debug actions are available only in Play Mode.\n" +
                    "Hit Play first.",
                    MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField("Zones", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            DrawButton("Reset to Zone 1", () => panel.Zone.JumpToZone(1));
            DrawButton("Zone 5 (Safe)",   () => panel.Zone.JumpToZone(5));
            DrawButton("Zone 29",         () => panel.Zone.JumpToZone(29));
            DrawButton("Zone 30 (Super)", () => panel.Zone.JumpToZone(30));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(6);

            EditorGUILayout.LabelField("Currency", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            DrawButton("+100 Gold",  () => panel.Currency.AddCurrency(100));
            DrawButton("+1000 Gold", () => panel.Currency.AddCurrency(1000));
            DrawButton("Set 0 Gold", () => panel.Currency.SetCurrency(0));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(6);

            EditorGUILayout.LabelField("Panel Triggers", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            DrawButton("Trigger Bomb",    () => panel.Zone.Bomb());
            DrawButton("Trigger Cashout", () => panel.Zone.Cashout());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(6);

            EditorGUILayout.LabelField("Reward Bag", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            DrawButton("Add Random x10",  () => AddRandomReward(panel, 10));
            DrawButton("Add Random x100", () => AddRandomReward(panel, 100));
            DrawButton("Clear Bag",       () => panel.Bag.ClearRewards());
            EditorGUILayout.EndHorizontal();
        }

        private void DrawButton(string label, System.Action action)
        {
            if (GUILayout.Button(label, GUILayout.Height(28)))
                action?.Invoke();
        }

        private void AddRandomReward(DebugPanel panel, int amount)
        {
            if (panel.TestRewards == null || panel.TestRewards.Length == 0)
            {
                UnityEngine.Debug.LogWarning("[DebugPanel] No test rewards assigned in inspector.");
                return;
            }

            var reward = panel.TestRewards[Random.Range(0, panel.TestRewards.Length)];
            panel.Bag.AddReward(reward, amount);
            UnityEngine.Debug.Log($"[DebugPanel] Added {amount}x {reward.DisplayName} to bag.");
        }
    }
}
#endif