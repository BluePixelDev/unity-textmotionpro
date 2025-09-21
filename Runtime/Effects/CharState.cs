namespace BP.TextMotionPro
{
    /// <summary>
    /// Contains state of currently processed character.
    /// </summary>
    public readonly ref struct CharState
    {
        public readonly int characterIndex;
        public readonly float animationTime;

        public CharState(int characterIndex, float animationTime)
        {
            this.characterIndex = characterIndex;
            this.animationTime = animationTime;
        }
    }
}
