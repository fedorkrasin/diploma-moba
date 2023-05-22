namespace Core.Characters.Spells
{
    public struct SpellCooldownState
    {
        public int SpellId;
        public float SpellReadyTime;

        public SpellCooldownState(int spellId, float spellReadyTime)
        {
            SpellId = spellId;
            SpellReadyTime = spellReadyTime;
        }
    }
}