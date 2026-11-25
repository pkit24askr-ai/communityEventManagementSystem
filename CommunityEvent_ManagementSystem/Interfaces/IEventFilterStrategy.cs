using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Interfaces
{
    public interface IEventFilterStrategy
    {
        List<CommunityEvent> Filter(List<CommunityEvent> events);
    }
}