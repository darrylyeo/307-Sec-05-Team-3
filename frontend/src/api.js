/*
POST user/login/authenticate	
User login.

Event
API	Description
GET api/event/getAllEvents	
Retrieves all events.

GET api/event/getEventById?EventId={EventId}	
Retrieves event given its corresponding ID.

POST api/event/postNewEvent	
Posts a new event.

PUT api/event/updateEvent	
Modifies an existing event.

DELETE api/event/deleteEvent	
Deletes an event.

GET api/event/getEventsByUserId?UserId={UserId}	
Lists all of a user's events.

POST api/event/getEventsByTimeRange	
List all events within a speficied time range.

UserRegistration
API	Description
POST user/register/newUser	
Register a new user.
*/

const get = path => fetch(`https://campusnowbackend.azurewebsites.net/` + path).then(result => result.json())
const lowercaseKeys = obj => Object.fromEntries(Object.entries(obj).map(
	([k, v]) => [k[0].toLowerCase() + k.slice(1), v]
))

export const API = {
    user: {
        login: {
            authenticate(){

            }
        },
        register: {
            newUser(){

            }
        }	
    },

    event: {
        getAllEvents(){
            return get(`api/event/getAllEvents`)
				.catch(e => ({
					"EventRecords": [
						{
							"ListingId": 1,
							"UserId": 2,
							"Title": "sample string 3",
							"Description": "sample string 4",
							"StartTime": "2020-02-18T11:33:37.6211161+00:00",
							"EndTime": "2020-02-18T11:33:37.6211161+00:00",
							"LocX": 7.1,
							"LocY": 8.1
						},
						{
							"ListingId": 1,
							"UserId": 2,
							"Title": "sample string 3",
							"Description": "sample string 4",
							"StartTime": "2020-02-18T11:33:37.6211161+00:00",
							"EndTime": "2020-02-18T11:33:37.6211161+00:00",
							"LocX": 7.1,
							"LocY": 8.1
						}
					]
				}))
				.then(_ => _.EventRecords).then(_ => _.map(lowercaseKeys))
        },
        getEventById(EventId){
            return get(`api/event/getEventById?EventId={EventId}`)
        },
        postNewEvent(){
        },
        updateEvent(){
        },
        deleteEvent(){

        },

        getEventsByUserId(userID){
            `api/event/getEventsByUserId?UserId=${UserId}`
        },

        getEventsByTimeRange(){
            post()
        },

        newUser(){
            post()
        }
    }
}