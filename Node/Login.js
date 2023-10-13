const express = require('express');
const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');
const app = express();
//npm해야될거 , init -y, install express, install package.json
app.use(bodyParser.urlencoded({extended : flase}));
app.use(bodyParser.json());

const users = [
    {id : 'user1', password : 'password1'},
    {id : 'user2', password : 'password2'},
];

const secretkey = 'secretkey';

function verifyToken(req, res, next){
    const token = req.headers.authorization;

    if(!token){
        return res.status(403).json({message : 'no token provided'});
    }

    jwt.verify(token, secretkey, (err,decode) =>{
        if(err){
            return res.status(401).json({message: 'failed to authenticate token'});
        }
        req.decode == decode;
        next();
    })
}

app.post('/LeeHan/login', (req, res) => {
    const {id, password} = req.body;

    const user = users.find(u => u.id === id && u.password === password);

    if(user){
        const token = jwt.sign({id : user.id}, secretkey , {expiresIn: '1h'});
        console.log(token);
        res.status(200).json({token});
    }
    elseres.status(401).json({message : 'login failed'})
})