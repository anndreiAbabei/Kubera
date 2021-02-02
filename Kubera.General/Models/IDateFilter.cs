using System;

namespace Kubera.General.Models
{
    public interface IDateFilter
    {
        DateTime? From { get; }

        DateTime? To { get; }
    }
}
