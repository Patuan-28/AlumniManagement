using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamWeb.EventService;
using ExamWeb.Models;
using ExamWeb.Interfaces;

namespace ExamWeb.Services
{
    public class EventRepository : IEventRepository
    {
        private readonly EventServiceClient _eventServiceClient;
        public EventRepository()
        {
            _eventServiceClient = new EventServiceClient();
        }

        public IEnumerable<EventDTO> GetEvents()
        {
            var data = _eventServiceClient.GetEvents();
            return data;
        }
        public EventDTO GetEventByID(int eventID)
        {
            var data = _eventServiceClient.GetEventByID(eventID);
            return data;
        }

        public void InsertEvent(EventModel eventModel)
        {
            var result = Mapping.Mapper.Map<EventDTO>(eventModel);
            _eventServiceClient.InsertEvent(result);
        }

        public void UpsertEvent(EventModel eventModel)
        {
            var result = Mapping.Mapper.Map<EventDTO>(eventModel);
            _eventServiceClient.UpsertEvent(result);
        }

        public void UpdateEvent(EventModel eventModel)
        {
            var result = Mapping.Mapper.Map<EventDTO>(eventModel);
            _eventServiceClient.UpdateEvent(result);
        }
        public void DeleteEvent(int eventID)
        {
            _eventServiceClient.DeleteEvent(eventID);
        }
    }
}