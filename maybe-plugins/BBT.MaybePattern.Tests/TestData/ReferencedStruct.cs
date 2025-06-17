namespace BBT.MaybePattern.Tests.TestData
{
    /// <summary>
    /// Used for test purposes.
    /// </summary>
    public struct ReferencedStruct
    {
        /// <summary>
        /// Gets or sets the referencing struct.
        /// </summary>
        public BaseStruct ReferencingStruct { get; set; }

        /// <summary>
        /// Gets or sets the referencing class.
        /// </summary>
        public BaseClass ReferencingClass { get; set; }
    }
}
