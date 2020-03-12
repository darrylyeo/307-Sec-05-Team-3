<script>
	import { API } from './api.js'

	export let currentUserToken

	let isSigningUp = true

	let signUpForm = {
		username: '',
		password: '',
		firstName: '',
		lastName: '',
	}

	const onSignupSubmit = async function(e){
		const result = await API.user.newUser({
			...signUpForm,
			// joinDate: Date.now()
		})
		if(result)
			$currentUserToken = result
	}


	let logInForm = {
		username: '',
		password: '',
	}

	const onLoginSubmit = async function(e){
		const result = await API.login.authenticate(logInForm)
		console.log(result)
		$currentUserToken = result
	}
</script>

{#if isSigningUp}
	<div class="sticky">
		<h2>Sign Up</h2>
		<button on:click={() => isSigningUp = false}>I already have an account</button>
	</div>
	<form on:submit|preventDefault={onSignupSubmit}>
		<label>
			<span>First Name</span>
			<input type="text" bind:value={signUpForm.firstName} placeholder="Musty">
		</label>
		<label>
			<span>Last Name</span>
			<input type="text" bind:value={signUpForm.lastName} placeholder="Mustang">
		</label>
		<label>
			<span>Username</span>
			<input type="text" bind:value={signUpForm.username} placeholder="musty_mustang">
		</label>
		<label>
			<span>Password</span>
			<input type="password" bind:value={signUpForm.password} placeholder="6 characters or more"><!-- pattern="......" -->
		</label>
		<hr>
		<input type="submit" disabled={!Object.values(signUpForm).every(_ => _)} value="Sign Up">
	</form>
{:else}
	<div class="sticky">
		<h2>Log In</h2>
		<button on:click={() => isSigningUp = true}>I don't have an account</button>
	</div>
	<form on:submit|preventDefault={onLoginSubmit}>
		<label>
			<span>Username</span>
			<input type="text" bind:value={logInForm.username} placeholder="musty_mustang">
		</label>
		<label>
			<span>Password</span>
			<input type="password" bind:value={logInForm.password} placeholder="6 characters or more">
		</label>
		<hr>
		<input type="submit" disabled={!Object.values(logInForm).every(_ => _)} value="Log In">
	</form>
{/if}


<style>
	div {
		display: flex;
	}
</style>