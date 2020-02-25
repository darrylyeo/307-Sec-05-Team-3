<script>
	import { onMount } from 'svelte'
	import leaflet from 'leaflet'
 
	export let getEvents
	
	let mapElement
	let markers = []

	onMount(async () => {
		const map = leaflet.map(mapElement, {
			center: [35.3013, -120.6620],
			zoom: 17
		})
		console.log(map)
		
		leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
			attribution: '' // '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
		}).addTo(map)

		const events = await getEvents
		
		$: markers = events.map(event =>
			leaflet.marker([event.locX, event.locY])
				.addTo(map)
				.on('click', e => {
					console.log(events)
				})
				.bindPopup(`
					<h3>event.title</h3>
					<p>event.description</p>
				`)
		)
	})
</script>

<div id="map" bind:this={mapElement}></div>

<style>
	#map {
		font: inherit;
	}

	:global(.leaflet-popup-content, .leaflet-popup-content p) {
		margin: 0;
	}
</style>