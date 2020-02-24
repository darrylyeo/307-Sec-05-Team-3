<script>
	import { onMount } from 'svelte'
	import leaflet from 'leaflet'
 
	let mapElement

	console.log(onMount);onMount(() => {
		const map = leaflet.map(mapElement, {
			center: [51.505, -0.09],
			zoom: 13
		})
		console.log(map)
	})
</script>

<div id="map" bind:this={mapElement}></div>