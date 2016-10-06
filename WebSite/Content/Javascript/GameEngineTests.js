QUnit.test(
    "RemoveBlackPositions_NoMatches_NoRemovals",
    function (assert) {
        var code = [1, 1, 1, 1];
        var guess = [2, 2, 2, 2];

        var result = RemoveBlackPositions(code, guess);

        assert.equal(result.NewCode.length, 4);
        assert.equal(result.NewGuess.length, 4);
        for (i = 0; i < 4; ++i) {
            assert.equal(result.NewCode[i], 1);
            assert.equal(result.NewGuess[i], 2);
        }
    }
);

QUnit.test(
    "RemoveBlackPositions_RemoveMatchFromMiddle",
    function (assert) {
        var code = [6, 1, 7, 8];
        var guess = [2, 1, 3, 4];

        var result = RemoveBlackPositions(code, guess);

        assert.equal(result.NewCode.length, 3);
        assert.equal(result.NewGuess.length, 3);

        assert.equal(result.NewCode[0], 6);
        assert.equal(result.NewGuess[0], 2);

        assert.equal(result.NewCode[1], 7);
        assert.equal(result.NewGuess[1], 3);

        assert.equal(result.NewCode[2], 8);
        assert.equal(result.NewGuess[2], 4);
    }
);

QUnit.test(
    "RemoveBlackPositions_RemoveAll",
    function (assert) {
        var code = [6, 1, 7, 8];
        var guess = [6, 1, 7, 8];

        var result = RemoveBlackPositions(code, guess);

        assert.equal(result.NewCode.length, 0);
        assert.equal(result.NewGuess.length, 0);
    }
);



QUnit.test(
    "RemoveWhitePositions_RemoveNone",
    function (assert) {
       var code = [4, 5, 6];
       var guess = [1, 2, 3];

       var result = RemoveWhitePositions(code, guess);

       assert.equal(result.NewCode.length, 3);
       assert.equal(result.NewGuess.length, 3);

       assert.equal(result.NewCode[0], 4);
       assert.equal(result.NewCode[1], 5);
       assert.equal(result.NewCode[2], 6);
       assert.equal(result.NewGuess[0], 1);
       assert.equal(result.NewGuess[1], 2);
       assert.equal(result.NewGuess[2], 3);
    }
);


QUnit.test(
    "RemoveWhitePositions_Remove2",
    function (assert) {
        var code = [4, 5, 6];
        var guess = [6, 2, 4];

        var result = RemoveWhitePositions(code, guess);

        assert.equal(result.NewCode.length, 1);
        assert.equal(result.NewGuess.length, 1);

        assert.equal(result.NewCode[0], 5);
        assert.equal(result.NewGuess[0], 2);
    }
);

QUnit.test(
    "RemoveWhitePositions_RemoveAll",
    function (assert) {
        var code = [4, 5, 6];
        var guess = [6, 4, 5];

        var result = RemoveWhitePositions(code, guess);

        assert.equal(result.NewCode.length, 0);
        assert.equal(result.NewGuess.length, 0);
    }
);


QUnit.test(
    "CalculateGuessResult_NoMatches",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [9, 8, 7, 6];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 0);
        assert.equal(result.WhiteCount, 0);
    }
);

QUnit.test(
    "CalculateGuessResult_1_White",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [4, 8, 7, 6];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 0);
        assert.equal(result.WhiteCount, 1);
    }
);

QUnit.test(
    "CalculateGuessResult_Multiple_Whites",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [4, 3, 2, 6];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 0);
        assert.equal(result.WhiteCount, 3);
    }
);

QUnit.test(
    "CalculateGuessResult_All_Whites",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [4, 3, 2, 1];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 0);
        assert.equal(result.WhiteCount, 4);
    }
);


QUnit.test(
    "CalculateGuessResult_1_Black",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [9, 2, 8, 7];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 1);
        assert.equal(result.WhiteCount, 0);
    }
);

QUnit.test(
    "CalculateGuessResult_Multiple_Blacks",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [9, 2, 8, 4];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 2);
        assert.equal(result.WhiteCount, 0);
    }
);

QUnit.test(
    "CalculateGuessResult_All_Blacks",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 2, 3, 4];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 4);
        assert.equal(result.WhiteCount, 0);
    }
);

QUnit.test(
    "CalculateGuessResult_Some_Whites_And_Blacks",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 2, 4, 8];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 2);
        assert.equal(result.WhiteCount, 1);
    }
);

QUnit.test(
    "CalculateGuessResult_All_Whites_And_Blacks",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 2, 4, 3];

        var result = CalculateGuessResult(code, guess);
        assert.equal(result.BlackCount, 2);
        assert.equal(result.WhiteCount, 2);
    }
);

QUnit.test(
    "ResultIsConsistentWithCode_Neither_Match",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 3, 7, 8];
        var result = { BlackCount: 2, WhiteCount: 0 };
        assert.notOk(ResultIsConsistentWithCode(code, guess, result));
    }
);

QUnit.test(
    "ResultIsConsistentWithCode_Just_White_Matches",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 3, 7, 8];
        var result = { BlackCount: 0, WhiteCount: 1 };
        assert.notOk(ResultIsConsistentWithCode(code, guess, result));
    }
);

QUnit.test(
    "ResultIsConsistentWithCode_Just_Black_Matches",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 3, 7, 8];
        var result = { BlackCount: 1, WhiteCount: 3 };
        assert.notOk(ResultIsConsistentWithCode(code, guess, result));
    }
);

QUnit.test(
    "ResultIsConsistentWithCode_Both_Match",
    function (assert) {
        var code = [1, 2, 3, 4];
        var guess = [1, 3, 7, 8];
        var result = { BlackCount: 1, WhiteCount: 1 };
        assert.ok(ResultIsConsistentWithCode(code, guess, result));
    }
);

QUnit.test(
    "AreResultsConsistentWithCode_First_Not_Consistent",
    function (assert) {

        assert.notOk(
            AreResultsConsistentWithCode(
                [1, 2, 3, 4],
                [
                    {
                        Guess: [1, 3, 6, 7],
                        Result: {BlackCount: 1, WhiteCount: 2}
                    },
                    {
                        Guess: [1, 3, 6, 7],
                        Result: { BlackCount: 1, WhiteCount: 1 }
                    },
                ]
            )
        );        
    }
);

QUnit.test(
    "AreResultsConsistentWithCode_Last_Not_Consistent",
    function (assert) {

        assert.notOk(
            AreResultsConsistentWithCode(
                [1, 2, 3, 4],
                [
                    {
                        Guess: [1, 3, 6, 7],
                        Result: { BlackCount: 1, WhiteCount: 1 }
                    },
                    {
                        Guess: [1, 3, 6, 7],
                        Result: { BlackCount: 1, WhiteCount: 2 }
                    },
                ]
            )
        );
    }
);

QUnit.test(
    "AreResultsConsistentWithCode_Both_Consistent",
    function (assert) {

        assert.ok(
            AreResultsConsistentWithCode(
                [1, 2, 3, 4],
                [
                    {
                        Guess: [1, 3, 6, 7],
                        Result: { BlackCount: 1, WhiteCount: 1 }
                    },
                    {
                        Guess: [1, 3, 6, 7],
                        Result: { BlackCount: 1, WhiteCount: 1 }
                    },
                ]
            )
        );
    }
);

QUnit.test(
    "AddCompleteCodeToList_DumpResults",
    function (assert) {
        assert.expect(0);
        var completeCodeList = [];        
        AddCompleteCodeToList(completeCodeList, [], 3, 3);
        
        var i;
        for (i = 0; i < completeCodeList.length; ++i) {
            console.log(StringifyCode(completeCodeList[i]));
        }
    }
);


QUnit.test(
    "CodesConsistentWithGuessResults_DumpResults",
    function (assert) {
        assert.expect(0);
        CodesConsistentWithGuessResults(4, 4,
            [
                {
                    Guess: [3, 2, 3, 1],
                    Result: {
                        BlackCount: 0,
                        WhiteCount: 3
                    }
                },
                {
                    Guess: [0, 2, 1, 0],
                    Result: {
                        BlackCount: 1,
                        WhiteCount: 2
                    }
                }
            ]

        ).forEach(
            function (currentValue, index, array) {
                console.log(StringifyCode(currentValue));
            }
            
        );
    }
);

