using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ExamWeb.EventService;
using ExamWeb.Models;

namespace ExamWeb.Interfaces
{
    public interface IEventRepository
    {
        IEnumerable<EventDTO> GetEvents();

        EventDTO GetEventByID(int eventID);

        void InsertEvent(EventModel eventModel);
        void UpsertEvent(EventModel eventModel);
        void UpdateEvent(EventModel eventModel);
        void DeleteEvent(int eventID);
    }
}
