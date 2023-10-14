using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols  //차근차근 수정할 것
{
    public class Packets 
    {
        public class common
        {
            public int cmd;
            public string message;
        }
        public class req_data : common
        {
            public int id;
            public string data;
        }
        //서버를 통신할 node에도 변수명을 같게 해야지 데이터 전달이 된다.
        public class res_data : common
        {
            public req_data[] result;
        }

        public class ConstructionStatusResponse
        {
            public string message;
        }
    }
    
}
