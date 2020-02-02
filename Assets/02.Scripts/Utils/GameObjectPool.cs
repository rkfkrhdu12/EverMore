using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
    // 게임 오브젝트 풀링
{
    short count; // 갯수
    public delegate T Func(); // 대리자
    Func create_fn; // 풀링 시 작동하는 함수

    Stack<T> objects; // 스택 형식

    public GameObjectPool(short count, Func fn)
        // 생성자
    {
        this.count = count;
        this.create_fn = fn;        
        this.objects = new Stack<T>(this.count);
        allocate();

    }
    void allocate()
        // 할당 함수
    {
        for (int i = 0; i < this.count; ++i)
        {
            this.objects.Push(this.create_fn());
        }
    }
    public T pop()
        // 넣기
    {
        if (this.objects.Count <= 0)
        {
            allocate();
        }
        return this.objects.Pop();
    }
    public void push(T obj)
        // 빼오기
    {
        this.objects.Push(obj);
    }

}
