$(document).ready(function ()  
{  
    $('#calendar').fullCalendar({  
        header:  
        {  
            left: 'prev,next today',  
            center: 'title',  
            right: 'month,agendaWeek,agendaDay'  
        },  
        buttonText: {  
            today: 'today',  
            month: 'month',  
            week: 'week',  
            day: 'day'  
        },  
        views: {
            timelineHours: {
                type: 'timeline',
                duration: { hours: 8 },
                buttonText: 'hours'
            }
        },
        
  
        events: function (start, end, timezone, callback)  
        {  
            $.ajax({  
                url: '/Branches/2/Schedule/GetCalendarEvents/2020/48/',  
                type: "GET",  
                dataType: "JSON",  
  
                success: function (result)  
                {  
                    
                    var events = [];  
  
                    $.each(result, function (i, data)  
                    {
                        events.push(  
                       {  
                           title: data.title,  
                           description: data.description,  
                           start: moment(data.start).format('YYYY-MM-DD'),  
                           end: moment(data.end).format('YYYY-MM-DD'),
                           duration: data.duration,
                           backgroundColor: "#9501fc",  
                           borderColor: "#fc0101"  
                       });  
                    });  
  
                    callback(events);  
                }  
            });  
        },  
  
        //eventRender: function (event, element)  
        //{
            
        //    element.qtip(  
        //    {  
        //        content: event.description  
        //    });  
        //},  
  
        editable: false  
    });  
}); 
