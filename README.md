# RouletteApi

Process to play

- Create user with username and initial money.
- Create roulette with name and isOpen = false.
- Open roulette by id
- Add bets with roulette id, user id, roulette game id, bet value, bet roulette number, bet roulette color

//////////////////////////////////////////////////////////
1. Roulette create

  POST: https://localhost:44379/api/Roulettes

  Body
        {
            "name": "Roulette Name",
            "isOpen": false
        }

//////////////////////////////////////////////////////////
2. Open Roulette

  GET: https://localhost:44379/api/Roulettes/Open/{id}

//////////////////////////////////////////////////////////
3. Bet
  
  POST: https://localhost:44379/api/Bets
  
  Body
        {
            "IdRoullete": 1,
            "IdUser": 1,
            "Value": 2000,
            "Number": 30,
            "Color": "r",
            "Game": 1
        }
  
//////////////////////////////////////////////////////////
4. Close Roulette

  GET: https://localhost:44379/api/Roulettes/Close/{id}

//////////////////////////////////////////////////////////
5. Roulettes list

  GET: https://localhost:44379/api/Roulettes

//////////////////////////////////////////////////////////
6. Create User
  
  POST: https://localhost:44379/api/Users
  
  Body
        {
            "UserName": "Usuario 1",
            "Money": 50000
        }
