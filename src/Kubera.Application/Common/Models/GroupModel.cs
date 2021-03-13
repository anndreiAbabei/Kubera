using System;
using System.Diagnostics;

namespace Kubera.Application.Common.Models
{
    [DebuggerDisplay("Id: {Id}, Code: {Code}, Name: {Name}")]
    public class GroupModel : GroupInputModel
    {
        public Guid Id { get; set; }
    }
}
