const express = require('express');
const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');
const app = express();
//npm해야될거 , init -y, install express, install package.json, i jsonwebtoken
app.use(bodyParser.urlencoded({extended : false}));
app.use(bodyParser.json());

const users = [
    
];

const secretkey = '231014';

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
        //console.log(token);
        res.status(200).json({token});
    }
    else{
        res.status(401).json({message : 'login failed'})
    } 
});

app.get('/LeeHan/protected', verifyToken, (req, res) =>{
    res.status(200).json({message: 'This is a protected endpoint', user: req.decode});
});


const PORT = process.env.PORT || 3030;
app.listen(PORT , ()=>{
    console.log('server is running on port 3030');
});

app.post('/LeeHan/sign_up', (req, res)=>{
    const {id, password} = req.body;

    const user = users.find(u => u.id === id);
    
    if(user){
        res.status(409).json({message : '이미 존재하는 계정입니다.'});
    }
    else{   
        //객체를 만들 때 양식을 지켜줘서 만들어야됨 
        //const user = users.find(u => u.id === id) 이렇게 돼있어서
        users.push({id: id, password: password});
        res.status(200).json({message : '회원가입에 성공하였습니다.'});
    }
    console.log(user);
    console.log(users);
});