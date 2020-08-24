using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class MatrixBarcodeGenerator : UdonSharpBehaviour {
    //---------------------------------------------------------
    // dataTable
    //---------------------------------------------------------
    private int[] AlphaToIntTable = new int[] {
        1, 2, 4, 8, 16, 32, 64, 128, 29, 58, 116, 232, 205, 135, 19, 38, 
        76, 152, 45, 90, 180, 117, 234, 201, 143, 3, 6, 12, 24, 48, 96, 192, 
        157, 39, 78, 156, 37, 74, 148, 53, 106, 212, 181, 119, 238, 193, 159, 35, 
        70, 140, 5, 10, 20, 40, 80, 160, 93, 186, 105, 210, 185, 111, 222, 161, 
        95, 190, 97, 194, 153, 47, 94, 188, 101, 202, 137, 15, 30, 60, 120, 240, 
        253, 231, 211, 187, 107, 214, 177, 127, 254, 225, 223, 163, 91, 182, 113, 226, 
        217, 175, 67, 134, 17, 34, 68, 136, 13, 26, 52, 104, 208, 189, 103, 206, 
        129, 31, 62, 124, 248, 237, 199, 147, 59, 118, 236, 197, 151, 51, 102, 204, 
        133, 23, 46, 92, 184, 109, 218, 169, 79, 158, 33, 66, 132, 21, 42, 84, 
        168, 77, 154, 41, 82, 164, 85, 170, 73, 146, 57, 114, 228, 213, 183, 115, 
        230, 209, 191, 99, 198, 145, 63, 126, 252, 229, 215, 179, 123, 246, 241, 255, 
        227, 219, 171, 75, 150, 49, 98, 196, 149, 55, 110, 220, 165, 87, 174, 65, 
        130, 25, 50, 100, 200, 141, 7, 14, 28, 56, 112, 224, 221, 167, 83, 166, 
        81, 162, 89, 178, 121, 242, 249, 239, 195, 155, 43, 86, 172, 69, 138, 9, 
        18, 36, 72, 144, 61, 122, 244, 245, 247, 243, 251, 235, 203, 139, 11, 22, 
        44, 88, 176, 125, 250, 233, 207, 131, 27, 54, 108, 216, 173, 71, 142, 1,
    };

    private int[] IntToAlphaTable = new int[] {
        0, 0, 1, 25, 2, 50, 26, 198, 3, 223, 51, 238, 27, 104, 199, 75, 
        4, 100, 224, 14, 52, 141, 239, 129, 28, 193, 105, 248, 200, 8, 76, 113, 
        5, 138, 101, 47, 225, 36, 15, 33, 53, 147, 142, 218, 240, 18, 130, 69, 
        29, 181, 194, 125, 106, 39, 249, 185, 201, 154, 9, 120, 77, 228, 114, 166, 
        6, 191, 139, 98, 102, 221, 48, 253, 226, 152, 37, 179, 16, 145, 34, 136, 
        54, 208, 148, 206, 143, 150, 219, 189, 241, 210, 19, 92, 131, 56, 70, 64, 
        30, 66, 182, 163, 195, 72, 126, 110, 107, 58, 40, 84, 250, 133, 186, 61, 
        202, 94, 155, 159, 10, 21, 121, 43, 78, 212, 229, 172, 115, 243, 167, 87, 
        7, 112, 192, 247, 140, 128, 99, 13, 103, 74, 222, 237, 49, 197, 254, 24, 
        227, 165, 153, 119, 38, 184, 180, 124, 17, 68, 146, 217, 35, 32, 137, 46, 
        55, 63, 209, 91, 149, 188, 207, 205, 144, 135, 151, 178, 220, 252, 190, 97, 
        242, 86, 211, 171, 20, 42, 93, 158, 132, 60, 57, 83, 71, 109, 65, 162, 31, 
        45, 67, 216, 183, 123, 164, 118, 196, 23, 73, 236, 127, 12, 111, 246, 108, 
        161, 59, 82, 41, 157, 85, 170, 251, 96, 134, 177, 187, 204, 62, 90, 203, 
        89, 95, 176, 156, 169, 160, 81, 11, 245, 22, 235, 122, 117, 44, 215, 79, 
        174, 213, 233, 230, 231, 173, 232, 116, 214, 244, 234, 168, 80, 88, 175,
    };

    private int[] AlignmentPatternLocations = new int[] {
        0, 0, 18, 22, 26, 30, 34
    };

    private int[] DataCapacityTable = new int[] {
        0, 0, 0, 0,
        16, 19, 9, 13,
        28, 34, 16, 22,
        44, 55, 26, 34,
        64, 80, 36, 48,
        86, 108, 46, 62,
        108, 136, 60, 76,
    };

    private int[] ErrorCodewordTable = new int[] {
        0, 0, 0, 0,
        10, 7, 17, 13,
        16, 10, 28, 22,
        26, 15, 22, 18,
        18, 20, 16, 26,
        24, 26, 22, 18,
        16, 18, 28, 24,
    };

    private int[] AlphanumericEncodeTable = new int[] {
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, 36, -1, -1, -1, 37, 38, -1, -1,
        -1, -1, 39, 40, -1, 41, 42, 43,  0,  1,
         2,  3,  4,  5,  6,  7,  8,  9, 44, -1, 
        -1, -1, -1, -1, -1, 10, 11, 12, 13, 14,
        15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
        25, 26, 27, 28, 29, 30, 31, 32, 33, 34,
        35, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    };

    private int[] DataLimitForAlphanumericMode = new int[] {
        -1, -1, -1, -1,
        20, 25, 10, 16,
        38, 47, 20, 29,
        61, 77, 35, 37,
        90, 114, 50, 67,
        122, 154, 64, 87,
        154, 195, 84, 108,
    };
    private int[] DataLimitForBytesMode = new int[] {
        -1, -1, -1, -1,
        14, 17, 7, 11,
        26, 32, 14, 20,
        42, 53, 24, 32,
        62, 78, 34, 46,
        84, 106, 44, 60,
        106, 134, 58, 74,
    };

    private int[] InitMatrixBarcodev1 = new int[] {
        16647164, 1052782, 8436596, 383904, 3064065,  522927, 14680064, 131072, 
        0, 8388608, 32, 64, 260096, 1064960, 12189701, 13631534, 
        8388868, 4064, 0,
    };

    private int[] InitMatrixBarcodev2 = new int[] {
        16646207, 12648464, 7241739, 12009477, 14393346, 15470593, 522922, 16646144, 
        512, 0, 128, 0, 32, 0, 8, 0, 
        2, 63488, 4211775, 8399376, 4198667, 10489733, 13631490, 15204353, 
        262144, 16646144, 0
    };

    private int[] InitMatrixBarcodev3 = new int[] {
        16646147, 16519168, 1076864, 47988, 1499, 10485806, 12648449, 522922, 
        11526144, 2, 0, 0, 32768, 0, 32, 0, 
        0, 524288, 0, 512, 0, 0, 8388608, 0, 
        8192, 16252992, 279544, 10768, 4194576, 12189711, 8769536, 11904, 
        260, 15, 14680064, 0,
    };

    private int[] InitMatrixBarcodev4 = new int[] {
        16646144, 4178176, 4206, 8388619, 12009472, 383904, 748, 1048577, 
        522922, 11206144, 0, 131072, 0, 0, 8388608, 0, 
        32, 0, 0, 2048, 0, 0, 131072, 0, 
        0, 8388608, 0, 32, 0, 0, 2048, 0, 
        0, 131072, 16252992, 17471, 8388650, 1064960, 1117088, 3973, 
        13631488, 190464, 260, 0, 16646144, 0,
    };

    private int[] InitMatrixBarcodev5 = new int[] {
        16646144, 261136, 16, 7241728, 47988, 5, 14393344, 11969, 
        1, 522922, 11186144, 0, 512, 0, 0, 128, 
        0, 0, 32, 0, 0, 8, 0, 0, 
        2, 0, 0, 0, 8388608, 0, 0, 2097152, 
        0, 0, 524288, 0, 0, 131072, 0, 0, 
        32768, 0, 0, 8192, 63488, 4194308, 4454400, 10768, 
        4194305, 1096192, 3973, 13631488, 11904, 1, 262144, 4064, 
        0, 0,
    };

    private int[] InitMatrixBarcodev6 = new int[] {
        16646144, 16321, 0, 1076864, 11, 12009472, 1499, 10485760, 
        191504, 1, 522922, 11184894, 0, 2, 0, 0, 
        0, 32768, 0, 0, 32, 0, 0, 0, 
        524288, 0, 0, 512, 0, 0, 0, 8388608, 
        0, 0, 8192, 0, 0, 8, 0, 0, 
        0, 131072, 0, 0, 128, 0, 0, 0, 
        2097152, 0, 0, 2048, 0, 0, 2, 0, 
        16252992, 68, 4161536, 10768, 4194304, 1117088, 15, 8769536, 
        2, 15204352, 260, 0, 65024, 0, 0
    };

    private bool[] FuncMatrixBarcodev1 = new bool[] {
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false, true, true, true, true, true, true, true, true,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,
    };

    private bool[] FuncMatrixBarcodev2 = new bool[] {
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
    };

    private bool[] FuncMatrixBarcodev3 = new bool[] {
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
    };

    private bool[] FuncMatrixBarcodev4 = new bool[] {
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,

    };
    
    private bool[] FuncMatrixBarcodev5 = new bool[] {
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
    };
    
    private bool[] FuncMatrixBarcodev6 = new bool[] {
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true, true, true, true,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
        false,false,false,false,false,false, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false, true, true, true, true, true,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
         true, true, true, true, true, true, true, true, true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
    };

    //---------------------------------------------------------
    // field
    //---------------------------------------------------------

    // Input row text to generate Matrix Barcode  
    public Text text;

    // Place an object attached with the 'Encoder' class
    public Encoder encoder;

    // Place a material attached with 'MatrixBarcodeShader'
    public MeshRenderer render;

    // Error correction level
    // Other values are supported (default value is 1 (L))
    public int ecLevel = 1;

    // Encording mode
    // Now only encodeMode=4 is supported
    public int encodeMode = 4;

    private string rowText = "";
    private int version;
    private int MatrixBarcodeSize;
    private int[] data = new int[71];
    private byte[] message = new byte[140];
    private float spendTime;

    //---------------------------------------------------------
    // functions
    //---------------------------------------------------------
    void Start() {
        if (ecLevel < 0 || 4 < ecLevel) {
            ecLevel = 1;
        }

        if (/* encodeMode != 2 || */ encodeMode != 4) {
            encodeMode = 4;
        }
    }

    void Update() {
        if (rowText != text.text) {
            rowText = text.text;
            CreateMatrixBarcode(ref rowText);
        }
    }

    public void CreateMatrixBarcode(ref string rowText) {
        InitMessage();
        EncodeData(ref rowText);
        MatrixBarcodeSize = (version - 1) * 4 + 21;
        var g = CreateDataBinary(message, version);

        var funcPosition = SelectMatrixBarcodeVersion();
        int maskPattern = PutMatrixBarcodeData(funcPosition, g);
        PutFormatData(data, maskPattern);

        render.material.SetInt("_matrix_barcode_size", MatrixBarcodeSize);
        for (int i = 0; i < (MatrixBarcodeSize * MatrixBarcodeSize - 1) / 24 + 1; i++) {
            render.material.SetInt(string.Format("_value{0}", i + 1), data[i]);
        }
    }

    //---------------------------------------------------------
    // internal functions
    //---------------------------------------------------------
    private void InitMessage() {
        for (int i = 0; i < message.Length; i++) {
            message[i] = (byte) 0;
        }
    }

    private int SelectLevel(int rowDataLength) {
        int version = 1;
        int limit = 0;
        while (version < 7) {
            if (encodeMode == 2) {
                limit = DataLimitForAlphanumericMode[4 * version + ecLevel];
            } else if (encodeMode == 4) {
                limit = DataLimitForBytesMode[4 * version + ecLevel];
            } else {
                limit = -1;
            }

            if (limit >= rowDataLength) {
                return version;
            }
            version++;
        }

        return version;
    }

    private bool[] ToBit(int a, int length) {
        bool[] n = new bool[length];
        for (int i = 0; i < length; i++) {
            bool b = (a % 2 == 1);
            n[length - i - 1] = b; 
            a /= 2;
        }
        return n;
    }

    private bool[] EncodeAndPutAlpanumeric(ref string rowData, int iter) {
        // TODO: Put data into message in this function
        char[] charArray = rowData.ToUpper().ToCharArray();
        int L = charArray.Length;
        int bitSize = (L / 2) * 11 + (L % 2) * 6;
        bool[] encodedData = new bool[bitSize];

        for (int i = 0; i < L / 2; i++) {
            int t = 0;
            t += AlphanumericEncodeTable[(int) charArray[i * 2]] * 45;
            t += AlphanumericEncodeTable[(int) charArray[i * 2 + 1]];
            var b = ToBit(t, 11);

            for (int j = 0; j < 11; j++) {
                encodedData[i * 11 + j] = b[j];
            }
        }
        
        if (L % 2 == 1) {
            int t = AlphanumericEncodeTable[(int) charArray[L - 1]];
            var b = ToBit(t, 6);
            for (int j = 0; j < 6; j++) {
                encodedData[11 * (L / 2) + j] = b[j];
            }
        }

        return encodedData;
    }

    private int EncodeAndPutBytes(byte[] a, int iter) {
        int s = iter % 8;
        for (int i = 0; i < a.Length; i++) {
            message[iter / 8] |= (byte) (a[i] >> s);
            message[(iter / 8) + 1] |= (byte) ((a[i] & ((1 << s) - 1)) << (8 - s));
            iter += 8;
        }
        return iter;
    }

    private int AddIntToMessage(int data, int size, int iter) {
        for (int i = 0; i < size; i++) {
            if (((data >> (size - i - 1)) & 1) == 1) {
                message[iter / 8] |= (byte) (1 << (7 - (iter % 8)));
            }
            iter++;
        }

        return iter;
    }

    private void EncodeData(ref string rowData) {
        // note: now only supported encodeMode 4
        int iter = 0;
        iter = AddIntToMessage(encodeMode, 4, iter);

        bool[] encodedData;
        int dataLen = 0;
        // if (encodeMode == 2) {
        //     iter += 9;
        //     encodedData = EncodeAndPutAlpanumeric(ref rowData, iter);
        //     dataLen = rowData.Length;
        //     AddIntToMessage(dataLen, 9, 4);
        // } else if (encodeMode == 4) {
        if (encodeMode == 4) {
            iter += 8;
            var bytes = encoder.StringToU8(ref rowData);
            iter = EncodeAndPutBytes(bytes, iter);
            dataLen = bytes.Length;
            AddIntToMessage(dataLen, 8, 4);
        } else {
            return; 
        }

        version = SelectLevel(dataLen);
        if (version >= 7) {
            return;
        }

        int dataCapacity = DataCapacityTable[version * 4 + ecLevel];

        int terminaterLength = dataCapacity * 8 - iter;
        if (terminaterLength > 4) {
            terminaterLength = 4;
        }
        iter += terminaterLength;
        iter += (7 - ((iter - 1) % 8));

        iter /= 8;
        int paddingLength = dataCapacity - iter;
        for (int i = 0; i < paddingLength; i++) {
            if (i % 2 == 0) {
                message[iter] = (byte) 236;
            } else {
                message[iter] = (byte) 17;
            }
            iter++;
        }
    }

    private byte[] SliceArray(byte[] array, int begin, int end) {
        byte[] slice = new byte[end - begin];
        for (int i = 0; i < end - begin; i++) {
            slice[i] = array[begin + i];
        }

        return slice;
    }

    private byte[] Reverse(byte[] a) {
        byte[] r = new byte[a.Length];
        for (int i = 0; i < a.Length; i++) {
            r[i] = a[a.Length - i - 1];
        }
        return r;
    }

    private int AlphaToInt(int i) {
        return AlphaToIntTable[i];
    }

    private int IntToAlpha(int i) {
        return IntToAlphaTable[i];
    }

    private byte[] MultipulPolynomial(byte[] a, int b) {
        byte[] res = new byte[a.Length + 1];

        for (int i = 0; i < a.Length; i++) {
            res[i] ^= (byte) AlphaToInt((a[i] + b) % 255);
            res[i + 1] ^= (byte) AlphaToInt(a[i] % 255);
        }
        
        for (int i = 0; i < res.Length; i++) {
            res[i] = (byte) IntToAlpha(res[i]);
        }

        return res;
    }

    private byte[] CreateGeneratorPolynomial(int codewords) {
        byte[] generatePolynomial = new byte[1];
        for (int i = 0; i < codewords; i++) {
            generatePolynomial = MultipulPolynomial(generatePolynomial, i);
        }

        return generatePolynomial;
    }

    private byte[] GenerateErrorWords(byte[] message, int codewords) {
        byte[] messagePolynomial = new byte[codewords + message.Length];
        for (int i = 0; i < message.Length; i++) {
            messagePolynomial[i + codewords] = message[message.Length - i - 1];
        }

        var generatePolynomial = CreateGeneratorPolynomial(codewords);

        for (int k = 0; k < message.Length; k++) {
            int a = (int) messagePolynomial[messagePolynomial.Length - 1];
            if (a == 0) {
                messagePolynomial = SliceArray(messagePolynomial, 0, messagePolynomial.Length - 1);
                continue;
            }
            a = IntToAlpha(a);

            byte[] g = new byte[messagePolynomial.Length];
            for (int i = 0; i < generatePolynomial.Length; i++) {
                g[i + messagePolynomial.Length - codewords - 1] =
                    (byte) AlphaToInt((a + (int) generatePolynomial[i]) % 255);
            }

            for (int i = 0; i < messagePolynomial.Length; i++) {
                messagePolynomial[i] ^= g[i];
            }

            messagePolynomial = SliceArray(messagePolynomial, 0, messagePolynomial.Length - 1);
        }

        return Reverse(messagePolynomial);
    }

    private int[] FetchSeparatePosition(int dataLength, int version, int ecLevel) {
        switch (version) {
            case 2:
                return new int[] {0, dataLength};
            case 3:
                if (ecLevel <= 1) {
                    return new int[] {0, dataLength};
                }
                return new int[] {0, dataLength / 2, dataLength};
            case 4:
                if (ecLevel == 0) {
                    return new int[] {0, dataLength / 2, dataLength};
                } else if (ecLevel == 1) {
                    return new int[] {0, dataLength};
                } else if (ecLevel == 2) {
                    return new int[] {0, dataLength / 4, dataLength / 2, 3 * dataLength / 4, dataLength};
                }
                return new int[] {0, dataLength / 2, dataLength};
            case 5:
                if (ecLevel == 0) {
                    return new int[] {0, dataLength / 2, dataLength};
                } else if (ecLevel == 1) {
                    return new int[] {0, dataLength};
                } else if (ecLevel == 2) {
                    return new int[] {0, 11, 22, 34, 46};
                } 
                return new int[] {0, 15, 30, 46, 62};
            case 6:
                if (ecLevel == 1) {
                    return new int[] {0, dataLength / 2, dataLength};
                }
                return new int[] {0, dataLength / 4, dataLength / 2, 3 * dataLength / 4, dataLength};
            default:
                return new int[] {0, dataLength};
        }
    }

    private int[] CreateDataBinary(byte[] data, int version) {
        int dataLen = DataCapacityTable[version * 4 + ecLevel];
        int codewordsNum = ErrorCodewordTable[version * 4 + ecLevel];
        int[] separatePosition = FetchSeparatePosition(dataLen, version, ecLevel);
        int[] errorBlocks = new int[codewordsNum * (separatePosition.Length - 1)];

        for (int i = 0; i < separatePosition.Length - 1; i++) {
            var block = SliceArray(data, separatePosition[i], separatePosition[i + 1]);
            var errorWords = GenerateErrorWords(block, codewordsNum);

            for (int k = 0; k < codewordsNum; k++) {
                errorBlocks[i + k * (separatePosition.Length - 1)] = errorWords[k];
            }
        }

        int[] res = new int[dataLen + errorBlocks.Length];
        int blockLength = separatePosition[separatePosition.Length - 1] 
                          - separatePosition[separatePosition.Length - 2];
        int iter = 0;
        for (int i = 0; i < blockLength; i++) {
            for (int j = 0; j < separatePosition.Length - 1; j++) {
                if (i + separatePosition[j] >= separatePosition[j + 1]) {
                    continue;
                }
                res[iter] = data[i + separatePosition[j]];
                iter++;
            }
        }

        for (int i = 0; i < errorBlocks.Length; i++) {
            res[iter] = errorBlocks[i];
            iter++;
        }

        int binaryDataLength = res.Length * 8;
        if (version >= 2) {
            binaryDataLength += 7;
        }

        int[] binaryData = new int[binaryDataLength];
        for (int i = 0; i < res.Length; i++) {
            var b = ToBit(res[i], 8);
            for (int j = 0; j < 8; j++) {
                if (b[j]) {
                    binaryData[i * 8 + j] = 1;
                }
            }
        }
        return binaryData;
    }

    private bool[] SelectMatrixBarcodeVersion() {
        int[] initData;
        switch(version) {
            case 1:
                initData = InitMatrixBarcodev1;
                break;
            case 2:
                initData = InitMatrixBarcodev2;
                break;
            case 3:
                initData = InitMatrixBarcodev3;
                break;
            case 4:
                initData = InitMatrixBarcodev4;
                break;
            case 5:
                initData = InitMatrixBarcodev5;
                break;
            case 6:
                initData = InitMatrixBarcodev6;
                break;
            default:
                return new bool[0];
        }

        for (int i = 0; i < initData.Length; i++) {
            data[i] = initData[i];
        }

        switch(version) {
            case 1:
                return FuncMatrixBarcodev1;
            case 2:
                return FuncMatrixBarcodev2;
            case 3:
                return FuncMatrixBarcodev3;
            case 4:
                return FuncMatrixBarcodev4;
            case 5:
                return FuncMatrixBarcodev5;
            case 6:
                return FuncMatrixBarcodev6;
            default:
                return new bool[0];
        }
    }

    private int PutMatrixBarcodeData(bool[] funcPosition, int[] binaryData) {
        int p = MatrixBarcodeSize - 1;
        int q = MatrixBarcodeSize - 1;
        int dirct = -1;
        int dataIter = 0;
        int r = 0;
        int maskPattern = 3;

        while (p >= 0 && dataIter < binaryData.Length) {
            if (!funcPosition[p * MatrixBarcodeSize + q]) {
                int d = binaryData[dataIter];

                // mask (pattern: 3)
                if ((p + q) % 3 == 0) {
                    d = 1 - d;
                }

                if (d == 1) {
                    int t = p * MatrixBarcodeSize + q;
                    data[t / 24] |= 1 << (23 - (t % 24));
                }
                dataIter++;
            }

            if (q % 2 == r) {
                q--;
            } else {
                p += dirct;
                q++;

                if (p < 0) {
                    p = 0;
                    q -= 2;
                    dirct = 1;
                } else if (p >= MatrixBarcodeSize) {
                    p = MatrixBarcodeSize - 1;
                    q -= 2;
                    dirct = -1;
                }

                if (q == 6) {
                    q--;
                    r = 1;
                }
            }
        }

        return maskPattern;
    }

    private void PutFormatData(int[] MatrixBarcodeData, int maskPattern) {
        int d = (ecLevel << 13) + (maskPattern << 10);
        int t = d;
        int generateBits = 1335;
        while (t > 1024) {
            int r = 10;
            while ((t >> r) != 0) {
                r++;
            }
            t ^= generateBits << (r - 11);
        }
        d += t;
        d ^=  21522;

        int v = 0;
        for (int i = 0; i < 15; i++) {
            if (((d >> (14 - i)) & 1) == 1) {
            if (i < 6) {
                    v = 8 * MatrixBarcodeSize + i;
            } else if (i < 7) {
                    v = 8 * MatrixBarcodeSize + i + 1;
            } else {
                    v = 8 * MatrixBarcodeSize + MatrixBarcodeSize - (15 - i);
                }
                data[v / 24] |= 1 << (23 - (v % 24));

            if (i < 7) {
                    v = (MatrixBarcodeSize - (i + 1)) * MatrixBarcodeSize + 8;
            } else if (i < 9) {
                    v = (15 - i) * MatrixBarcodeSize + 8;
            } else {
                    v = (14 - i) * MatrixBarcodeSize + 8;
                }
                data[v / 24] |= 1 << (23 - (v % 24));
            }
        } 
    }

    //---------------------------------------------------------
    // debug
    //---------------------------------------------------------
    private void DebugPringMatrixBarcode(int[] MatrixBarcodeData) {
        string line = ""; 
        for (int i = 0; i < MatrixBarcodeSize; i++) {
            for (int j = 0; j < MatrixBarcodeSize; j++) {
                int t = MatrixBarcodeData[i * MatrixBarcodeSize + j];
                if (t == 0) {
                    line += " true,";
                } else if (t == 1) {
                    line += " true,";
                } else if (t == 2) {
                    line += " true,";
                } else {
                    line += "false,";
                }
            }
            line += "\n";
        }
        Debug.Log(line);
    }

    private void DebugPringByte(byte[] arr) {
        string line = "["; 
        for (int i = 0; i < arr.Length; i++) {
            line += string.Format("{0},", arr[i].ToString());
        }
        line += "]";
        Debug.Log(line);
    }

    private void DebugPringBool(bool[] arr) {
        string line = "["; 
        for (int i = 0; i < arr.Length; i++) {
            if (arr[i]) {
                line += "true,";
            } else {
                line += "false,";
            }
        }
        line += "]";
        Debug.Log(line);
    }

    private void DebugPringInt(int[] arr) {
        string line = "["; 
        for (int i = 0; i < arr.Length; i++) {
                line += string.Format("{0},", arr[i]);
        }
        line += "]";
        Debug.Log(line);
    }
}
