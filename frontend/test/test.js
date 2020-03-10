const puppeteer = require('puppeteer')

;(async () => {
	const browser = await puppeteer.launch({ headless: false })
	const page = await browser.newPage()
	await page.goto('localhost:5000', {waitUntil: 'networkidle2'})

	// Initial load
	await page.waitFor(20000)
	
	// Click
	await page.click('#events > div:nth-of-type(3)')
	await page.waitFor(5000)
	
	// Search
	await page.type('input[type=search]', 'Career', {delay: 200})
	await page.waitFor(5000)

	await page.keyboard.press('Ctrl');
	await page.keyboard.press('Backspace');

	// Done
 	// await browser.close()
})()