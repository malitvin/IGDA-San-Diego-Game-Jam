namespace UI.HUD
{
    public interface IHUDElement
    {
        HUDConfig _hudConfig { get; set; }
        void OnCreationComplete(HUDConfig hudConfig);
        void OnWaveChanged(LevelConfig.WaveDef def,int maxWaves);
        void OnEnemyCountChanged(int count);
    }
}
