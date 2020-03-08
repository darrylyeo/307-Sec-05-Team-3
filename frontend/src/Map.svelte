<script>
	import { onMount } from 'svelte'
	import leaflet from 'leaflet'
 
	export let getEvents, currentEvent
	
	let mapElement
	let markerForEvent = new Map()

	Array.prototype.mapToMap = function(mapFunction){
		return new Map(
			this.map(mapFunction).map((mappedValue, i) => [this[i], mappedValue])
		)
	}

	let map
	onMount(() => {
		map = leaflet.map(mapElement, {
			center: [35.3013, -120.6620],
			zoom: 17
		})
		.on('popupopen', e => {
			console.log('popupopen', e)
		})
		.on('popupclose', e => {
			console.log('popupclose', e)

			$currentEvent = undefined
		})
		console.log('map', map)
		
		leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
			attribution: '' // '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
		}).addTo(map)
	})

	$: if(map){
		getEvents.then(events => {
			markerForEvent = events.mapToMap(event =>
				leaflet.marker([event.locX, event.locY])
					.addTo(map)
					.on('click', function(e){
						console.log(e)
					
						$currentEvent = event
					})
					.bindPopup(`
						<h3>${event.title}</h3>
						<p>${event.description}</p>
					`)
			)
		})
	}

	$: if(map && $currentEvent){
		console.log('$currentEvent', $currentEvent)
		// Center map on the marker
		const marker = markerForEvent.get($currentEvent)
		map.fitBounds(leaflet.latLngBounds([ marker.getLatLng() ]))
	}
</script>

<div id="map" bind:this={mapElement}></div>

<style>
	#map {
		background: rgba(0, 0, 0, 0.05);
		font: inherit;
		overflow: visible;
		z-index: 0;
	}

	:global(.leaflet-popup-content-wrapper) {
		background-color: rgba(255, 255, 255, 0.75);
		backdrop-filter: blur(3px) hue-rotate(40deg);
		box-shadow: 0 0.05rem 0.3rem rgba(0, 0, 0, 0.25);
		border-radius: 0.65em;
		font-size: 0.75em;
		padding: 0;
	}
	:global(.leaflet-popup-content-wrapper *, .leaflet-popup-content-wrapper p) {
		margin: 0;
	}
	:global(.leaflet-popup-content) {
		padding: 0.65rem;

		display: grid;
		gap: 0.25rem;
	}
</style>