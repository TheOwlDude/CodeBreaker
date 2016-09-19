var colorMap = new Array("red", "blue", "yellow", "pink", "black", "green", "orange", "purple");
var codeLength = 4;
var colorCount = 8;
var guessOutcomes;
var selectedGuessIndex = 0;
var selectedCodeIndex = 0;

function click() {
	alert("Click");
}

function keydown(e) {
	var arrowKey = true;
    switch(e.keyCode) {
	    case 37:  //left
		    if (selectedCodeIndex == 0) selectedCodeIndex = codeLength - 1;
			else --selectedCodeIndex;
			break;
		case 38:  //up
		    if (guessOutcomes[selectedGuessIndex].Guess[selectedCodeIndex] == colorCount - 1) {
				guessOutcomes[selectedGuessIndex].Guess[selectedCodeIndex] = 0;				
			}
			else {
				guessOutcomes[selectedGuessIndex].Guess[selectedCodeIndex]++;
			}
			break;
		case 39:  //right
		    if (selectedCodeIndex == codeLength - 1) selectedCodeIndex = 0;
			else ++selectedCodeIndex;
			break;
		case 40:  //down
		    if (guessOutcomes[selectedGuessIndex].Guess[selectedCodeIndex] == 0) {
				guessOutcomes[selectedGuessIndex].Guess[selectedCodeIndex] = colorCount - 1;				
			}
			else {
				guessOutcomes[selectedGuessIndex].Guess[selectedCodeIndex]--;
			}
			break;
		default: arrowKey = false;
    }
	
	if (arrowKey) {
		e.preventDefault();
		renderGame(guessOutcomes);
	}
}

function newGame() {
	var firstGuess = [0, 0, 0, 0];
    guessOutcomes = [ {Guess: firstGuess} ];
	selectedGuessItem = 0;
	renderGame(guessOutcomes);
}

function renderGame(guessOutcomes) {
	
	var svg = document.getElementById("svg");
	
	var innerhtml = "";
	
	for(i = 0; i < guessOutcomes.length; ++i) {
		innerhtml += "<svg y='" + (i * 12) + "' height='10'>";
		var guess = guessOutcomes[i].Guess;
		for(j = 0; j < guess.length; ++j) {
			innerhtml += "<circle cx='" + (4 + 6 * j) + "' cy='5' fill='" + colorMap[guess[j]] + "' r='2' " ;
			if (i == guessOutcomes.length - 1) {
				innerhtml += "id='lastRowGuess_" + j + "'";
			}
			innerhtml += "/>";
		}
		if (i == guessOutcomes.length - 1) {
			innerhtml += "<rect id x='" + (1 + 6 * selectedCodeIndex) + "' y='2' width='6' height='6' fill='white' fill-opacity='0.05' stroke='black' />";
		}
		innerhtml += "</svg>";		
	}
	svg.innerHTML = innerhtml;

	      
}