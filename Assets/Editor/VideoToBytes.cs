using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class VideoToBytes : EditorWindow
{

    [MenuItem("Tools/Video加密", false, 200)]
    //加密资源
    public static void BuildVideoByTurnByte()
    {
        //要加密的视频资源目录
        string videoPath = Application.dataPath + "/../video/";
        //加密后的视频资源目录
        string videoOutPath = Application.dataPath + "/AssetBundles/video/";
        
        //临时目录空的话就创建
        if (Directory.Exists(videoOutPath))
        {
            //删除临时目录下的所有文件
            foreach (string filePath in Directory.GetFiles(videoOutPath))
            {
                File.Delete(filePath);
            }
        }
        else
        {
            //创建目录
            Directory.CreateDirectory(videoOutPath);
        }
        
 
        //示例 选取目录下的所有文件
        var files = Directory.GetFiles(videoPath, "*", SearchOption.AllDirectories);
        List<string> list = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            //筛选掉.meta文件 不做操作
            string ext = Path.GetExtension(files[i]);
            if (ext == ".meta")
                continue;
 
            //获取文件名
            string fileName = Path.GetFileNameWithoutExtension(files[i]);
            //将文件转换为字节数组
            byte[] bytes = File.ReadAllBytes(files[i]);
            //加密
            byte[] buffer_ed = TurnByte(bytes);
            //输出加密后的文件 后缀名随你喜欢取  这里以哥哥为例
            File.WriteAllBytes(videoOutPath + fileName + ".bytes", buffer_ed);
 
        }
 
        Debug.Log("资源加密成功");
    }
 
 
    public static byte[] TurnByte(byte[] input)
    {
 
        for (int i = 0; i < input.Length; i++)
        {
            //隐式类型转换
            byte currentByte = input[i];
            //去相反的二进制
            byte shiftedValue = (byte)(~currentByte);
            //替换
            input[i] = shiftedValue; 
        }
 
        return input;
    }
    
}
#endif
