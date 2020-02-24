<script>
	import { onMount } from 'svelte'
	import leaflet from 'leaflet'
 
	export let getEvents
	
	let mapElement
	let markers = []

	onMount(async () => {
		const map = leaflet.map(mapElement, {
			center: [51.505, -0.09],
			zoom: 13
		})
		console.log(map)

		const events = await getEvents
		
		markers = events.map(event =>
			leaflet.marker([50.5, 30.5]).addTo(map)
		)
	})
</script>

<div id="map" bind:this={mapElement}></div>