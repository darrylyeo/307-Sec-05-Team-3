const formatDate = (dateString, timeOnly = false) => {
	return new Date(dateString).toLocaleString('en-US', {
		...timeOnly ? {} : {dateStyle: 'medium'},
		timeStyle: 'short'
	})
	// .toLocaleString('en-US', { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' })
}

const datesAreSameDay = (d1, d2) =>
    d1.getFullYear() === d2.getFullYear() &&
    d1.getMonth() === d2.getMonth() &&
    d1.getDate() === d2.getDate()

export {
	formatDate,
	datesAreSameDay
}