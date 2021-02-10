namespace Kubera.General.Models
{
    public interface IPaging
    {
        /// <summary>
        /// Zero based page
        /// </summary>
        uint Page { get; }

        /// <summary>
        /// Zero based number of items
        /// </summary>
        uint Items { get; }

        /// <summary>
        /// Output result of paging
        /// </summary>
        IPagingResult Result { get; set; }
    }
}
