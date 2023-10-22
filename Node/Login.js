const express = require('express');
const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');
const app = express();
require('dotenv').config();  

const mysql = require('mysql');

const connection = mysql.createConnection({
    host: process.env.DB_HOST,
    port: process.env.DB_PORT,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME,
});

connection.connect((err) => {
    if(err){
        console.error("MYSQL 연결 오류 : " + err.stack);
        return;
    }

    console.log("연결되었슴다. 연결 ID : " + connection.threadId);

});
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


const PORT = process.env.PORT || 3000;
app.listen(PORT , ()=>{
    console.log(PORT);
    console.log('server is running on port 3000');
});

app.post('/LeeHan/sign_up', (req, res) => {
    const { id, password } = req.body;
    console.log(id, password);
    // 사용자가 이미 존재하는지 확인
    // MySQL 데이터베이스에 사용자 정보 삽입
    connection.query('INSERT INTO leehan_account (id, password) VALUES (?, ?)', [id, password], (err, results) => {
        if (err) {
            console.log(err);
            return res.status(500).json({ message: '이미 존재하는 계정입니다.' });
        }
        res.status(200).json({ message: '회원가입에 성공하였습니다.' });
    });

    connection.end((err) => {
        if(err){
            console.error('MYSQL 연결 종료 오류 : ' + err.stack);
            return;
        }
        console.log('MySQL 연결이 성공적으로 종료되었습니다.');
    });
});