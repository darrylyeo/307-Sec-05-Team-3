const formatDate = dateString => {
	return new Date(dateString).toLocaleString('en-US', {
		dateStyle: 'medium',
		timeStyle: 'short'
	})
	// .toLocaleString('en-US', { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' })
}

export {
	formatDate
}