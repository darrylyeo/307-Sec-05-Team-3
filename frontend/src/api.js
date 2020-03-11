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

const API_URL = `https://campusnowbackend.azurewebsites.net` // `http://campusnow.tech`

const GET = (path, options = {}) =>
	fetch(API_URL + '/' + path, {
		...options,
		...options.body ? console.log(upperCaseKeys(options.body))||{
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(upperCaseKeys(options.body))
		} : {}
	})
	.then(result => result.json())
	// .then(lowerCaseKeys)

const POST = (path, body, options) =>
	GET(path, {...options, body, method: 'POST'})

const PUT = (path, body, options) =>
	GET(path, {...options, body, method: 'PUT'})

const DELETE = (path, body, options) =>
	GET(path, {...options, body, method: 'DELETE'})

if(!Object.fromEntries) Object.fromEntries = iterable => {
	const obj = {}
	for(const [key, val] of [...iterable])
		obj[key] = val
    return obj
}

const lowerCaseKeys = obj => Object.fromEntries(Object.entries(obj).map(
	([k, v]) => [k[0].toLowerCase() + k.slice(1), typeof v === 'object' ? lowerCaseKeys(v) : v]
))
const upperCaseKeys = obj => Object.fromEntries(Object.entries(obj).map(
	([k, v]) => [k[0].toUpperCase() + k.slice(1), typeof v === 'object' ? upperCaseKeys(v) : v]
))

export const API = {
	login: {
		authenticate({username, password}){
			return POST(`api/login/authenticate`, upperCaseKeys({
				username,
				password
			}))
		},
		
		getCurrentUser(token){
			return GET(`api/login/getCurrentUser`, {
				// credentials: 'include',
				headers: {
					'Authorization': `Bearer ${token}`
				}
			})
			.then(_ => _.CurrentUser && lowerCaseKeys(_.CurrentUser))
		},

		logout(){
			return GET(`api/login/logout`).then(_ => _.Status)
		}
	},
	
	user: {
		newUser(user){
			// return POST(`user/newUser`, user)
			return POST(`api/user/newUser`, {
				newUserRecord: {
					// "UserId": 1,
					// "UserName": "sample string 2",
					// "Password": "sample string 3",
					// "FirstName": "sample string 4",
					// "LastName": "sample string 5",
					// "JoinDate": "2020-03-09T03:46:07.0745748+00:00",
					// "IsAdmin": 7,
					...user
				}
			})
		}
	},

	event: {
		getAllEvents(){
			return GET(`api/event/getAllEvents`)
				.catch(e => ({
					"Events": [
						{
							"ListingId": 1,
							"UserId": 2,
							"Title": "Career Fair",
							"Description": "Find recruiters looking for college students!",
							"StartTime": "2020-02-18T11:33:37.6211161+00:00",
							"EndTime": "2020-02-18T11:33:37.6211161+00:00",
							"LocX": 35.3013,
							"LocY": -120.6620
						},
						{
							"ListingId": 2,
							"UserId": 2,
							"Title": "sample string 3",
							"Description": "sample string 4",
							"StartTime": "2020-02-18T11:33:37.6211161+00:00",
							"EndTime": "2020-02-18T11:33:37.6211161+00:00",
							"LocX": 7.1,
							"LocY": 8.1
						},
						{
							"ListingId": 3,
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
				.then(_ => _.EventRecords).then(_ => _.map(lowerCaseKeys))
		},

		getEventById(EventId){
			return GET(`api/event/getEventById?EventId=${EventId}`)
				.then(_ => _.Events).then(_ => _.map(lowerCaseKeys))
		},

		postNewEvent(event, user){
			return POST(`api/event/postNewEvent`, {
				"NewEvent": upperCaseKeys({
					// "listingId": 1,
					"userId": user.userId,
					// "title": "sample string 3",
					// "description": "sample string 4",
					// "startTime": "2020-03-09T03:40:18.821177+00:00",
					// "endTime": "2020-03-09T03:40:18.821177+00:00",
					// "locX": 7.1,
					// "locY": 8.1,
					...event
				})
			})
		},

		updateEvent(event){
			return PUT(`api/event/updateEvent`, {
				"updatedEvent": {
					...event
				}
			})
		},

		deleteEvent(event){
			return DELETE(`api/event/deleteEvent`, {
				"eventIdToDelete": event.id
			})
		},

		getEventsByUser(user){
			return this.getEventsByUserId(user.id)
		},

		getEventsByUserId(userID){
			return GET(`api/event/getEventsByUserId?UserId=${userID}`)
		},

		getEventsByTimeRange(startTime, endTime){
			return POST(`api/event/getEventsByTimeRange`, {
				startTime,
				endTime
			})
		}
	}
}