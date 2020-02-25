<script>
	import { formatDate } from './date.js'

	export let event = {
		title: '[Event Title]',
		description: '[Event description]',
		startTime: new Date(),
		endTime: new Date()
	}
</script>

<div class="event">
	<h3>{event.title}</h3>
	<p>{event.description}</p>
	<date>{formatDate(event.startTime)} – {formatDate(event.endTime)}</date>
</div>