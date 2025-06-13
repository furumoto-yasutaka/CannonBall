/*******************************************************************************
*
*	�^�C�g���F	�G�t�F�N�g�Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	EffectContainer.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/17
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectContainer : SingletonMonoBehaviour<EffectContainer>
{
    [System.Serializable]
    public class EffectData
    {
        // ���O(�C���X�y�N�^�[����̔��ʗp)
        public string name = "";

        // �G�t�F�N�g�{��
        public GameObject Effect = null;
    }

    //=============================================================================
    //     �ϐ�
    //=============================================================================
    [SerializeField]
    private List<EffectData> EffectPrefabs = null;

    //=============================================================================
    //     �X�^�[�g
    //=============================================================================
    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        // �p�����̌Ăяo��
        base.Awake();

        // �G�t�F�N�g���o�^����Ă��邩�̔���
        bool registered = false;

        // �G�t�F�N�g���o�^����Ă��邩�`�F�b�N
        for (int i = 0; i < EffectPrefabs.Count; i++)
        {
            if(!EffectPrefabs[i].Effect)
            {
                // �G�t�F�N�g���o�^����Ă��Ȃ��ꍇ�͍폜���ĊԂ��l�߂�
                EffectPrefabs.RemoveAt(i);
            }
            else
            {
                // ��ł��o�^����Ă���ꍇ�͓o�^���L�^
                registered = true;
            }
        }

        // �G�t�F�N�g������o�^����Ă��Ȃ��ꍇ
        if (!registered)
        {
            Debug.Log(this.gameObject.name + " �̓G�t�F�N�g�̓o�^���������ߍ폜����܂���");

            // �I�u�W�F�N�g���폜
            Destroy(this.gameObject);
        }
    }

    //=============================================================================
    //     �G�t�F�N�g���Đ�
    //=============================================================================
    /// <summary> �G�t�F�N�g���Đ� </summary>
    /// <param name="name"> �Đ�����G�t�F�N�g�̖��O </param>
    /// <param name="pos"> �Đ�����G�t�F�N�g�̏����|�W�V���� </param>
    /// <param name="rotation"> �Đ�����G�t�F�N�g�̏�����]�� </param>
    /// <param name="parent"> �y�A�����g�ݒ肷��I�u�W�F�N�g </param>
    /// <param name="scaleRate"> ���������I�u�W�F�N�g�̃X�P�[���Ɋ|����{�� </param>
    public void EffectPlay(string name, Vector3 pos,
        Quaternion rotation = default, Transform parent = null, Vector3 scaleRate = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        GameObject obj = Instantiate(GetEffect(name), pos, rotation);
        if (scaleRate != default)
        {
            obj.transform.localScale = new Vector3(
                obj.transform.localScale.x * scaleRate.x,
                obj.transform.localScale.y * scaleRate.y,
                obj.transform.localScale.z * scaleRate.z);
        }
        ParticleSystem particle = obj.GetComponent<ParticleSystem>();
        particle.Play(true);
        particle.transform.SetParent(parent);
    }

    /// <summary> �G�t�F�N�g���Đ� </summary>
    /// <param name="refObj"> �G�t�F�N�g���i�[����I�u�W�F�N�g </param>
    /// <param name="name"> �Đ�����G�t�F�N�g�̖��O </param>
    /// <param name="pos"> �Đ�����G�t�F�N�g�̏����|�W�V���� </param>
    /// <param name="rotation"> �Đ�����G�t�F�N�g�̏�����]�� </param>
    /// <param name="parent"> �y�A�����g�ݒ肷��I�u�W�F�N�g </param>
    /// <param name="scaleRate"> ���������I�u�W�F�N�g�̃X�P�[���Ɋ|����{�� </param>
    public void EffectPlay(ref GameObject refObj, string name, Vector3 pos,
        Quaternion rotation = default, Transform parent = null, Vector3 scaleRate = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        refObj = Instantiate(GetEffect(name), pos, rotation);
        if (scaleRate != default)
        {
            refObj.transform.localScale = new Vector3(
                refObj.transform.localScale.x * scaleRate.x,
                refObj.transform.localScale.y * scaleRate.y,
                refObj.transform.localScale.z * scaleRate.z);
        }
        ParticleSystem particle = refObj.GetComponent<ParticleSystem>();
        particle.Play(true);
        particle.transform.SetParent(parent);
    }

    /// <summary> �G�t�F�N�g���Đ� </summary>
    /// <param name="particle"> �p�[�e�B�N���ݒ���i�[����I�u�W�F�N�g </param>
    /// <param name="name"> �Đ�����G�t�F�N�g�̖��O </param>
    /// <param name="pos"> �Đ�����G�t�F�N�g�̏����|�W�V���� </param>
    /// <param name="rotation"> �Đ�����G�t�F�N�g�̏�����]�� </param>
    /// <param name="parent"> �y�A�����g�ݒ肷��I�u�W�F�N�g </param>
    /// <param name="scaleRate"> ���������I�u�W�F�N�g�̃X�P�[���Ɋ|����{�� </param>
    public void EffectPlay(ref ParticleSystem particle, string name, Vector3 pos,
        Quaternion rotation = default, Transform parent = null, Vector3 scaleRate = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        GameObject obj = Instantiate(GetEffect(name), pos, rotation);
        if (scaleRate != default)
        {
            obj.transform.localScale = new Vector3(
                obj.transform.localScale.x * scaleRate.x,
                obj.transform.localScale.y * scaleRate.y,
                obj.transform.localScale.z * scaleRate.z);
        }
        particle = obj.GetComponent<ParticleSystem>();
        particle.Play(true);
        particle.transform.SetParent(parent);
    }

    //=============================================================================
    //     ���O����G�t�F�N�g������
    //=============================================================================
    /// <summary>
    /// ���O����G�t�F�N�g������
    /// </summary>
    /// <param name="name">�G�t�F�N�g�̖��O</param>
    private int GetIndex(string name)
    {
        // ���[�v��
        int loopIndex = 0;

        // �G�t�F�N�g�ꗗ�̒�����Y�����閼�O�̃G�t�F�N�g������
        foreach(EffectData effect in EffectPrefabs)
        {
            // �����֐���p���č��v�����ꍇ
            if(FindTitleMatch(effect, name))
            {
                // ���[�v�񐔂�߂�l�ŕԂ�
                return loopIndex;
            }

            // ���[�v�񐔉��Z����
            loopIndex++;
        }

        // �����ŊY���̃t�@�C����������Ȃ������ꍇ
        Debug.LogError("�w�肳�ꂽ���O�̃t�@�C���͓o�^����Ă��܂���");

        return -1;
    }

    //=============================================================================
    //     ���O�����ăG�t�F�N�g���擾
    //=============================================================================
    /// <summary>
    /// ���O�����ăG�t�F�N�g���擾
    /// </summary>
    /// <param name="name">�G�t�F�N�g�̖��O</param>
    public GameObject GetEffect(string name)
    {
        // ���O�������̏ꍇ�͏I��
        if (name == null)
        {
            return null;
        }

        // ���O����C���f�b�N�X������
        int index = GetIndex(name);

        // BGM�̎w��ɃG���[���N�������ꍇ�͏I��
        if (index < 0 || EffectPrefabs.Count <= index)
        {
            return null;
        }

        // �G�t�F�N�g�̃v���n�u��Ԃ�
        return EffectPrefabs[index].Effect;
    }

    //=============================================================================
    //     ���O�̈�v������
    //=============================================================================
    /// <summary> �G�t�F�N�g���������̓t�@�C�����ƈ�v���邩����</summary>
    /// <param name="Data"> �G�t�F�N�g�f�[�^ </param>
    /// <param name="name"> ���肷�閼�O </param>
    /// <returns> �G�t�F�N�g���������̓^�C�g�����ƈ�v������true��Ԃ� </returns>
    private bool FindTitleMatch(EffectData Data, string name)
    {
        if (Data.name.Equals(name) || Data.Effect.name.Equals(name)) { return true; }

        return false;
    }
}
