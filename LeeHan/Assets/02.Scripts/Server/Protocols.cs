using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols  //�������� ������ ��
{
    public class Packets 
    {
        public class common
        {
            public int cmd;
            public string message;
            public bool name_is_null = false;
            public string id;
            public string player_name;
        }
        public class req_data : common
        {
            public int id;
            public string data;
        }
        //������ ����� node���� �������� ���� �ؾ��� ������ ������ �ȴ�.
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
