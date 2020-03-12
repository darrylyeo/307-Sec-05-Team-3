<script>
	import { mapCenter, mapClick } from './map-state.js'
	import { DateTime } from './date.js'
	
	import { createEventDispatcher } from 'svelte'
	const dispatch = createEventDispatcher()

	let startTime = new DateTime(Date.now())
	let endTime = new DateTime(Date.now() + 60 * 60 * 1000)

	export let event = {
		title: '',
		description: '',
		
		startTime: Date.now(),
		endTime: Date.now() + 60 * 60 * 1000,

		locX: $mapCenter && $mapCenter.lat,
		locY: $mapCenter && $mapCenter.lng,
	}

	$: {
		startTime.set(event.startTime)
		endTime.set(event.endTime)
	}

	$: {
		event.startTime = startTime.toString()
		event.endTime = endTime.toString()
	}

	$: {
		event.locX = $mapClick.lat
		event.locY = $mapClick.lng
	}


	export let submitLabel
	
	const onSubmit = async function(e){
		dispatch('submit', event)
	}
</script>

<form on:submit|preventDefault={onSubmit}>
	<label>
		<span>Event Title</span>
		<h3><input type="text" bind:value={event.title} placeholder="Computing Career Fair"></h3>
	</label>

	<label>
		<span>Description</span>
		<textarea bind:value={event.description} placeholder="Meet with recruiters at the Multi-Activity Center" />
	</label>

	<label>
		<span>Start Time</span>
		<span>
			<input type="date" bind:value={startTime.date} />
			<input type="time" bind:value={startTime.time} />
		</span>
		<!-- <input type="text" bind:value={event.startTime} format="\d?\d:\d\d [AP]M" /> -->
	</label>

	<label>
		<span>End Time</span>
		<span>
			<input type="date" bind:value={endTime.date} />
			<input type="time" bind:value={endTime.time} />
		</span>
		<!-- <input type="text" bind:value={event.endTime} format="\d?\d:\d\d [AP]M" /> -->
	</label>

	<div>
		<span>Location (Click on the map)</span>
		<span>
			<input type="number" bind:value={event.locX} step=".000000000000001" placeholder="Latitude" />
			<input type="number" bind:value={event.locY} step=".000000000000001" placeholder="Longitude" />
		</span>
	</div>

	<button type="submit">{submitLabel}</button>
</form>