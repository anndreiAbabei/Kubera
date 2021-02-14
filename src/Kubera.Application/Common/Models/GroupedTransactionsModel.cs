using System.Collections.Generic;

namespace Kubera.Application.Common.Models
{
    public class GroupedTransactionsModel
    {
        public GroupModel Group { get; set; }

        public IEnumerable<TransactionModel> Transactions { get; set; }
    }
}