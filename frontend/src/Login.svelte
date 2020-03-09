<script>
	import { API } from './api.js'

	export let currentUser

	let isSigningUp = true

	let signUpForm = {
		username: '',
		password: '',
		firstName: '',
		lastName: '',
	}

	const onSignupSubmit = async function(e){
		const result = await API.user.register.newUser({
			...signUpForm,
			joinDate: Date.now()
		})
		console.log(result)
		$currentUser = result.user
	}


	let logInForm = {
		username: '',
		password: '',
	}

	const onLoginSubmit = async function(e){
		const result = await API.user.login.authenticate(logInForm)
		console.log(result)
		$currentUser = result.user
	}
</script>

{#if isSigningUp}
	<form on:submit|preventDefault={onSignupSubmit}>
		<div>
			<h2>Sign Up</h2>
			<button on:click={() => isSigningUp = false}>I have an account</button>
		</div>
		<label>
			<span>Username</span>
			<input type="text" bind:value={signUpForm.username}>
		</label>
		<label>
			<span>Password</span>
			<input type="password" bind:value={signUpForm.password}>
		</label>
		<label>
			<span>First Name</span>
			<input type="text" bind:value={signUpForm.firstName}>
		</label>
		<label>
			<span>Last Name</span>
			<input type="text" bind:value={signUpForm.lastName}>
		</label>
		<button type="submit" disabled={!!(logInForm.username && logInForm.password)}>Sign Up</button>
	</form>
{:else}
	<form on:submit|preventDefault={onLoginSubmit}>
		<h2>Log In</h2>
		<button on:click={() => isSigningUp = true}>I don't have an account</button>
		<label>
			<span>Username</span>
			<input type="text" bind:value={logInForm.username}>
		</label>
		<label>
			<span>Password</span>
			<input type="password" bind:value={logInForm.password}>
		</label>
		<button type="submit" disabled={!!(logInForm.username && logInForm.password)}>Log In</button>
	</form>
{/if}