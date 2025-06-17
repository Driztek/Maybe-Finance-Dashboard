namespace BBT.MaybePattern.Tests.TestData
{
    using System;

    /// <summary>
    /// Used for test purposes.
    /// </summary>
    [Serializable]
    public class TestDataStruct
    {
        /// <summary>
        /// Gets or sets the maybe.
        /// </summary>
        public MaybeStruct<int> MaybeStruct { get; set; }
    }
}
