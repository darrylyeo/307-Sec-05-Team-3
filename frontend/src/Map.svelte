<script>
	import { onMount } from 'svelte'
	import leaflet from 'leaflet'

	import { mapCenter, mapClick } from './map-state.js'

	import Event from './Event.svelte'
 
	export let events = [], currentEvent
	
	let mapElement
	let markerForEvent = new Map()
	let eventForMarker = new Map()
	// let eventElementForPopup = new WeakMap()

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
			console.log('popupopen', e, e.popup.getContent())

			/*const {popup} = e
			const marker = popup._source
			if(!eventElementForPopup.has(popup))
				eventElementForPopup.add(popup, new Event({
					target: popup.getContent(),
					props: {
						event: eventForMarker.get(marker)
					}
				}))*/
		})
		.on('popupclose', e => {
			console.log('popupclose', e)
			
			/*const {popup} = e
			if(eventElementForPopup.has(popup))
				eventElementForPopup.delete(popup)
			*/

			$currentEvent = undefined
		})
		.on('move', e => {
			console.log('move', e.latlng)
			$mapCenter = e.latlng
		})
		.on('click', e => {
			console.log('click', e.latlng)
			$mapClick = e.latlng
		})
		console.log('map', map)
		
		leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
			attribution: '' // '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
		}).addTo(map)
	})

	$: if(map){
		markerForEvent = $events.mapToMap(event => {
			const div = document.createElement('div')
			new Event({target: div, props: {event}})
			const marker = leaflet.marker([event.locX, event.locY])
				.addTo(map)
				.on('click', function(e){
					console.log(e)
				
					$currentEvent = event

					console.trace(Event+'')
					console.trace(this, this.getElement(), new Event({target: this.getElement(), props: {event}}))
				})
				.bindPopup(div)
			return marker
		})

		eventForMarker = new Map([...markerForEvent.entries()]
			.map(([event, marker]) => [marker,event]))
	}

	$: if(map && $currentEvent){
		console.log('$currentEvent', $currentEvent)
		// Center map on the marker
		const marker = markerForEvent.get($currentEvent)
		if(marker){
			map.fitBounds(leaflet.latLngBounds([ marker.getLatLng() ]))
		}
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

	:global(.leaflet-popup-content-wrapper, .leaflet-popup-tip) {
		background-color: rgba(255, 255, 255, 0.75);
		backdrop-filter: blur(3px) hue-rotate(40deg);
		box-shadow: 0 0.05rem 0.3rem rgba(0, 0, 0, 0.25);
	}
	:global(.leaflet-popup-content-wrapper) {
		border-radius: 0.65em;
		font-size: 0.75em;
		padding: 0;
	}
	:global(.leaflet-popup-content-wrapper *, .leaflet-popup-content-wrapper p) {
		margin: 0;
	}
	:global(.leaflet-popup-content) {
		background: none;
		box-shadow: none;
		/* padding: 0.65rem; */

		display: grid;
		gap: 0.25rem;
	}
	:global(.leaflet-popup-content .event) {
		background: none;
	}
</style>