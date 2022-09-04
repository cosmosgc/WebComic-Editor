window.onload = () => {
	keyboardCommands()
	initSpoiler()
	initSuggestionBox()
	randomNames()
}

const keyboardCommands = () => {
	// Next/Prev page
	var prevPage, nextPage = null
	nextPage = document.getElementById("next-command-link").href
	prevPage = window.location.href
	prevPage = prevPage.split('/')
	if(prevPage[prevPage.length - 1] == 1) {
		prevPage = null
	} else {
		prevPage[prevPage.length - 1] = Number(prevPage[prevPage.length - 1]) - 1
		prevPage = prevPage.join('/')
	}
	  
	document.addEventListener('keydown', async (e) => {

		switch (e.key) {
			case "ArrowRight":
				nextPage ? window.location.href = nextPage : null
				break
				
			case "ArrowLeft":
				prevPage ? window.location.href = prevPage : null
				break
				
			case " ":
			case "Spacebar":
				const spoilers = document.getElementsByClassName('spoiler')
				for (let spoiler of spoilers) {
					spoiler.querySelector('input').click()
				}
				break
				
			default:
				break
		}
	})
}

const initSpoiler = () => {
	const spoilers = document.getElementsByClassName('spoiler')

	console.log("Abrindo spoiler");
	for (let spoiler of spoilers) {
		const input = spoiler.querySelector('input')
		input.addEventListener("click", (e) => {
			const element = e.currentTarget
			element.parentNode.parentNode.classList.toggle("closed")
			element.parentNode.parentNode.classList.toggle("open")
			if(element.value == element.getAttribute("data-close")) {
				element.value = element.getAttribute("data-open")
			} else if(element.value == element.getAttribute("data-open")) {
				element.value = element.getAttribute("data-close")
			}
		})
	}
}

const initSuggestionBox = () => {
	const suggestionBox = document.getElementById("btnSuggestionSubmit")
	if (typeof(suggestionBox) != 'undefined' && suggestionBox != null) {
		suggestionBox.setAttribute("onclick", "submitCommand()")
	}
}

const randomNames = () => {
	const intervalTime = 40
	const elements = document.getElementsByClassName("glitch")
  const chars = "QWERTYUIOPASDFGHJKLZXCVBNM"
	var string = ''
	for (let element of elements) {
		setInterval(() => {
			string = ''
			for (let i = 0; i < 6; i++) {
				string += chars.charAt(Math.floor(Math.random() * chars.length))
			}
			element.innerHTML = string
		}, intervalTime)
	}
}

const submitCommand = async () => {
	const min = 1
	const max = 256
	const msgs = {
		gameui: {
			short: "\"You can't craft emptiness, bro. (Voidbound readers DNI)\"",
			long: "\"You're crafting cool shit, not re-writing Homestuck. Trim it down.\"",
			selection: "\"You gotta pick a crafting type if you want this suggestion to be, you know, considered.\"",
			error: "\"You fucked up. I dunno how, but you did.\"",
			success: "\"Your suggestion has been grafted to the branches of the Hyperthetical! Now let's see if it bears fruit...\""
		}
	}
	
	const suggestion = document.getElementById("txtSuggestion")
	const suggestionBox = document.getElementById("btnSuggestionSubmit")
	const select = document.getElementById("selectSuggestion")
	const selection = (typeof(select) != 'undefined' && select != null) ? select.options[select.selectedIndex] : null
	const confirm = document.getElementById("confirmSuggestionSubmit")
	const responses = msgs[suggestion.getAttribute("data-theme")]
	const data = { text: `${(selection == null || selection.value == 'null') ? '' : selection.text.toUpperCase() + ': '}${suggestion.value}` }
	if (suggestion.value.length < min || suggestion.value.length > max || selection == '' || selection.value == 'null') {
		// Too short/long/no selection
		if (suggestion.value.length < min) confirm.textContent = responses.short
		if (suggestion.value.length > max) confirm.textContent = responses.long
		if (selection != null && selection.value == 'null') confirm.textContent = responses.selection
		confirm.classList.remove("confirmSuggestionErrorAnim")
		void confirm.offsetWidth;
		confirm.classList.add("confirmSuggestionErrorAnim")
	} else if (suggestion.value.length >= min && suggestion.value.length <= max && suggestion.value != 'null') {
		console.log(data)
		// Success
		const res = await fetch(`https://api.deconreconstruction.com/suggestions`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(data)
		})
		
		if (res.status == 200) {
			select.style.display = 'none'
			suggestion.style.display = 'none'
			suggestionBox.style.display = 'none'
			confirm.textContent = responses.success
			confirm.classList.remove("confirmSuggestionErrorAnim")
			confirm.classList.remove("confirmSuggestionSuccessAnim")
			void confirm.offsetWidth;
			confirm.classList.add("confirmSuggestionSuccessAnim")
		} else {
			// Wtf did you do
			confirm.textContent = responses.error
			confirm.classList.remove("confirmSuggestionErrorAnim")
			void confirm.offsetWidth;
			confirm.classList.add("confirmSuggestionErrorAnim")
		}
		
	} else {
		// Wtf did you do
		confirm.textContent = responses.error
		confirm.classList.remove("confirmSuggestionErrorAnim")
		void confirm.offsetWidth;
		confirm.classList.add("confirmSuggestionErrorAnim")
	}
	
	
	
	
	
	/*var xhr = new XMLHttpRequest();
	var res = "";
    xhr.open("POST", "https://kohi.la/vasterror/utils/submissions", true);
    xhr.setRequestHeader("Content-type","application/x-www-form-urlencoded");
    var params = `command=${command.value}`;
    xhr.send(params);
	if (command.value <= 0 || command.value > 64) {
		res = "Don't like that."
	} else {
		res = "Thank you for your input. Me, myself and I have already taken it into good consideration."
	}
	command.value = "";
	console.log(command.value);
	confirm.classList.remove("confirmSubmitAnim");
	void response.offsetWidth;
	confirm.textContent = res;
	confirm.classList.add("confirmSubmitAnim");*/
}