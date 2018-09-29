namespace Common.Pooling
{
    /// <summary>
    /// User: MaximusLit
    /// Date: 7/14/2018
    ///  Pooling Object IPoolable Interface w/ Methods for pooling
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// When this pooled object is created
        /// </summary>
        void OnInit(string key,GenericPooler pooler);

        /// <summary>
        /// Invoked when the object is instantiated.
        /// </summary>
        void OnPoolCreate();

        /// <summary>
        /// Invoked when the object is grabbed from the object pool.
        /// </summary>
        void OnPoolGet();

        /// <summary>
        /// Invoked when the object is released back to the object pool.
        /// </summary>
        void OnPoolRelease();

        /// <summary>
        /// Invoked when the object is reused.
        /// </summary>
        void OnPoolReuse();
    }
}
