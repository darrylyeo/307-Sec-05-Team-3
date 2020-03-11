<script>
	import { API } from './api.js'
	import { mapCenter, mapClick } from './map-state.js'

	const dateKey = Symbol()
	const timeKey = Symbol()

	const newEvent = {
		title: '',
		description: '',
		startTime: {
			[dateKey]: new Date().toISOString().split(/[A-Z]/)[0],
			[timeKey]: new Date().toISOString().split(/[A-Z]/)[1],
			toString(){
				return new Date(this.date, this.time)
			}
		},
		endTime: {
			[dateKey]: new Date().toISOString().split(/[A-Z]/)[0],
			[timeKey]: new Date().toISOString().split(/[A-Z]/)[1],
			toString(){
				return new Date(this.date, this.time)
			}
		},
		locX: $mapCenter && $mapCenter.lat,
		locY: $mapCenter && $mapCenter.lng,
	}

	$: {
		newEvent.locX = $mapClick.lat
		newEvent.locY = $mapClick.lng
	}

	const onSubmit = async function(e){
		const result = await API.event.postNewEvent(newEvent, {userId: 9})
		console.log(result)
	}
</script>

<form on:submit|preventDefault={onSubmit}>
	<label>
		<span>Event Title</span>
		<h3><input type="text" bind:value={newEvent.title} placeholder="Computing Career Fair"></h3>
	</label>

	<label>
		<span>Description</span>
		<textarea bind:value={newEvent.description} placeholder="Meet with recruiters at the Multi-Activity Center" />
	</label>

	<label>
		<span>Start Time</span>
		<span>
			<input type="date" bind:value={newEvent.startTime[dateKey]} />
			<input type="time" bind:value={newEvent.startTime[timeKey]} />
		</span>
		<!-- <input type="text" bind:value={newEvent.startTime} format="\d?\d:\d\d [AP]M" /> -->
	</label>

	<label>
		<span>End Time</span>
		<span>
			<input type="date" bind:value={newEvent.endTime[dateKey]} />
			<input type="time" bind:value={newEvent.endTime[timeKey]} />
		</span>
		<!-- <input type="text" bind:value={newEvent.endTime} format="\d?\d:\d\d [AP]M" /> -->
	</label>

	<label>
		<span>Location (Click on the map)</span>
		<span><input type="number" bind:value={newEvent.locX} step=".000000000000001" placeholder="Latitude" />, <input type="number" bind:value={newEvent.locY} step=".000000000000001" placeholder="Longitude" /></span>
	</label>

	<button type="submit">Post Event</button>
</form>