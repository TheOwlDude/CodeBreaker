var colorMap = new Array("red", "blue", "yellow", "pink", "black", "green", "orange", "purple");
var codeLength = 4;
var colorCount = 8;
var code;
var guessOutcomes;
var selectedCodeIndex = 0;
var gameWon = false;

function click() {
	alert("Click");
}

function keydown(e) {

    if (gameWon) return;
    var selectedGuessIndex = guessOutcomes.length - 1;
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


function GetIntValueOfInputWithinBounds(lowerBound, upperBound, defaultValue, elementId) {
    var element = document.getElementById(elementId);
    var valueAsNumber = Number(element.value);
    if (isNaN(valueAsNumber)) {
        element.value = defaultValue;
        return defaultValue;
    }
    else {
        valueAsNumber = Math.floor(valueAsNumber);
        if (valueAsNumber < lowerBound || valueAsNumber > upperBound) {
            element.value = defaultValue;
            return defaultValue;
        }
        else {
            return valueAsNumber;
        }
    }
}


function clearCheat() {
    var consistentCodesCell = document.getElementById("divConsistentCodes");
    consistentCodesCell.innerHTML = "";

    var bestGuessCell = document.getElementById("divBestGuessCodes");
    bestGuessCell.innerHTML = "";
}

function newGame() {
    gameWon = false;
    selectedCodeIndex = 0;

    clearCheat();

    codeLength = GetIntValueOfInputWithinBounds(2, 8, 4, "tbCodeLength");
    colorCount = GetIntValueOfInputWithinBounds(2, 8, 6, "tbColorCount");

    code = new Array(codeLength);
    for(i = 0; i < codeLength; ++i) {
        code[i] = Math.floor(Math.random() * colorCount); 
    }

	var firstGuess = createNewGuess();
    guessOutcomes = [ {Guess: firstGuess} ];
	renderGame(guessOutcomes);
}

function guess() {

    clearCheat();

    var guess =
    {
        CodeLength: codeLength,
        ColocCount: colorCount,
        Code: code,
        Guess: guessOutcomes[guessOutcomes.length - 1].Guess
    }

    var stringifiedJson = JSON.stringify(guess);
    $.ajax(
        {
            method: "Post",
            url: "../CodeBreaker/Guess",
            contentType: "application/json",
            data: stringifiedJson,
        }
    ).done(guessDoneCallback)
}

function addOutcome(result) {
    
    var guessWithoutOutcome = guessOutcomes[guessOutcomes.length - 1];
    guessWithoutOutcome.Outcome = { BlackCount: result.BlackCount, WhiteCount: result.WhiteCount };

    bestGuessToggled();
    consistentCodesToggled();

    if (result.BlackCount != codeLength) {
        var nextGuess = createNewGuess();
        guessOutcomes[guessOutcomes.length] = { Guess: nextGuess };
        selectedCodeIndex = 0;
    }
    else {
        gameWon = true;
    }

    renderGame();
}

function createNewGuess() {
    var nextGuess = new Array(codeLength);
    for (i = 0; i < codeLength; ++i) nextGuess[i] = 0;
    return nextGuess;
}

function guessDoneCallback(data, textStatus, jqXHR) {
    if (textStatus != "success") {
        alert("Guess done callback received non success status code " + textStatus);
    }
    else {
        addOutcome(data);
    }
}

function renderGame() {
	
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

        if (typeof guessOutcomes[i].Outcome === "undefined") {

        }
        else {
            for (b = 0; b < guessOutcomes[i].Outcome.BlackCount; ++b) {
                innerhtml += "<circle cx='" + (4 + 6 * guess.length + 2 * b) + "' cy='3' fill='black' r='1' />"
            }
            for (w = 0; w < guessOutcomes[i].Outcome.WhiteCount; ++w) {
                innerhtml += "<circle cx='" + (4 + 6 * guess.length + 2 * w) + "' cy='6' fill='gray' r='1' />"
            }
        }

		if (i == guessOutcomes.length - 1 && !gameWon) {
			innerhtml += "<rect id x='" + (1 + 6 * selectedCodeIndex) + "' y='2' width='6' height='6' fill='white' fill-opacity='0.05' stroke='black' />";
		}
		innerhtml += "</svg>";		
	}
	svg.innerHTML = innerhtml;

	if (gameWon) {
	    var cheatCell = document.getElementById("cheatCell");
	    cheatCell.innerHTML += "<img src='Image/fireworks2.gif' />";
	}
}


function consistentCodesToggled() {
    var cbConsistentCodes = document.getElementById("cbConsistentCodes");
    if (cbConsistentCodes.checked) {
        getConsistentCodes();
    }
    else {
        var divConsistentCodes = document.getElementById("divConsistentCodes");
        divConsistentCodes.innerHTML = "";
    }
}

function bestGuessToggled() {
    var cbBestGuess = document.getElementById("cbBestGuess");
    if (cbBestGuess.checked) {
        getBestGuess();
    }
    else {
        var divBestGuess = document.getElementById("divBestGuessCodes");
        divBestGuess.innerHTML = "";
    }
}


function getConsistentCodes() {

    var outcomes = [];
    for (i = 0; i < guessOutcomes.length; ++i) {
        if (typeof guessOutcomes[i].Outcome === "undefined") break;

        outcomes[i] = { Guess: guessOutcomes[i].Guess, BlackCount: guessOutcomes[i].Outcome.BlackCount, WhiteCount: guessOutcomes[i].Outcome.WhiteCount }
    }


    var consistentCodesRequest =
    {
        CodeLength: codeLength,
        ColorCount: colorCount,
        GameInfo: outcomes
    }

    var stringifiedJson = JSON.stringify(consistentCodesRequest);

    $.ajax(
        {
            method: "Post",
            url: "../CodeBreaker/ConsistentCodes",
            contentType: "application/json",
            data: stringifiedJson,
        }
    ).done(getConsistentCodesDoneCallback)
}


function getConsistentCodesDoneCallback(data, textStatus, jqXHR) {

    if (textStatus != "success") {
        alert("Consistent code done callback received non success status code " + textStatus);
        return;
    }

    var divConsistentCodes = document.getElementById("divConsistentCodes");    
    var innerhtml = "<div><label>Total Consistent Codes: " + data.ConsistentCodes.length + "</label></div>";

    for (i = 0; i < data.ConsistentCodes.length; ++i) {
        innerhtml += "<svg y='" + (i * 22) + "' height='20'>";
        var guess = data.ConsistentCodes[i];
        for (j = 0; j < guess.length; ++j) {
            innerhtml += "<circle cx='" + (8 + 16 * j) + "' cy='10' fill='" + colorMap[guess[j]] + "' r='6' />";
        }
        innerhtml += "</svg>"
    }

    divConsistentCodes.innerHTML = innerhtml;
}




function getBestGuess() {

    var outcomes = [];
    for (i = 0; i < guessOutcomes.length; ++i) {
        if (typeof guessOutcomes[i].Outcome === "undefined") break;

        outcomes[i] = { Guess: guessOutcomes[i].Guess, BlackCount: guessOutcomes[i].Outcome.BlackCount, WhiteCount: guessOutcomes[i].Outcome.WhiteCount }
    }


    var bestGuessRequest =
    {
        CodeLength: codeLength,
        ColorCount: colorCount,
        GameInfo: outcomes
    }

    var stringifiedJson = JSON.stringify(bestGuessRequest);

    $.ajax(
        {
            method: "Post",
            url: "../CodeBreaker/BestGuess",
            contentType: "application/json",
            data: stringifiedJson,
        }
    ).done(getBestGuessDoneCallback)
}


function getBestGuessDoneCallback(data, textStatus, jqXHR) {

    if (textStatus != "success") {
        alert("Best guess done callback received non success status code " + textStatus);
        return;
    }

    var divBestGuessCodes = document.getElementById("divBestGuessCodes");

    var innerhtml = "";
    for (i = 0; i < data.BestGuesses.length; ++i) {
        innerhtml += "<div><svg y='" + (i * 22) + "' height='20'>";
        var guess = data.BestGuesses[i].Guess;
        for (j = 0; j < guess.length; ++j) {
            innerhtml += "<circle cx='" + (8 + 16 * j) + "' cy='10' fill='" + colorMap[guess[j]] + "' r='6' />";
        }
        innerhtml += "</svg><label>" + data.BestGuesses[i].ExpectedPossibilities + "</label></div>"
    }

    divBestGuessCodes.innerHTML = innerhtml;
}
