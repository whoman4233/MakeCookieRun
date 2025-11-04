# MakeCookieRun
내일배움캠프 유니티 12기 유니티 입문 팀프로젝트

  
# 지우개 똥 대탈출!

## 프로젝트 소개

* **제목**: 지우개 똥 대탈출
* **장르**: 러닝 액션
* **플랫폼**: Android / PC
* **설명**
    > 열심히 뭉쳤더니 지우개똥이 생명을 얻었다!  
    > 책상 위에 가득한 장애물들을 피해 도망쳐보자!  

## 주요 기능

* **플레이어 조작**: 점프, 더블 점프, 슬라이드
* **입력 지원**: 키보드 (`Space`, `Shift`) 및 모바일 터치 버튼 동시 지원
* **게임 시스템**: HP 및 점수 시스템, 게임오버 및 재시작 로직
* **동적 맵 생성**: JSON 기반의 장애물 패턴을 동적으로 생성 (`MapManager`)
* **오브젝트 풀링**: 장애물과 아이템을 재활용하여 성능 최적화 (`ObstaclePool`, `ItemPool`)
* **펫 시스템**: `ScriptableObject`를 활용한 펫 선택 및 인게임 적용
* **사운드 관리**: BGM/SFX 재생 및 볼륨 조절 (`SoundManager`)
* **UI 상태 관리**: `enum`과 `BaseUI`를 활용한 체계적인 UI 패널 전환

## 조작 방법

| 동작 | 키보드 | 화면 버튼 |
| :--- | :--- | :--- |
| 점프 / 더블 점프 | `↑` 또는 `Space` | `Jump` 버튼 |
| 슬라이드 (유지) | `↓` 또는 `Shift` | `Slide` 버튼 (Hold) |


## 스크립트 상세 설명

### Manager (핵심 시스템 관리)

| 스크립트 | 설명 |
| :--- | :--- |
| `GameManager.cs` | 게임의 전반적인 상태 (시작, 일시정지, 게임오버)를 관리하는 싱글톤. 플레이어 HP, 점수 등 핵심 데이터를 저장하고 씬 로드 시 `OnSceneLoaded`를 통해 씬에 맞는 BGM과 UI 상태를 요청 |
| `UIManager.cs` | 모든 UI 패널(`Title`, `Play` 등)을 총괄하는 싱글톤 매니저. `EventManager`로부터 `UIState` 변경 요청을 구독하고, `ChangeState`를 통해 `BaseUI`를 상속받는 자식 UI들의 활성화 여부를 제어 |
| `EventManager.cs` | **(핵심)** `static event`를 사용한 중앙 이벤트 중계소. `Request...` 메서드로 이벤트를 게시하고, 다른 매니저들이 이벤트를 구독하여 처리. 스크립트 간의 결합도를 낮추는 핵심 역할 |
| `SoundManager.cs` | BGM/SFX 재생을 전담하는 싱글톤. `DontDestroyOnLoad`로 씬 전환 시에도 유지되며, `Dictionary`로 사운드 클립을 관리. `PlayerPrefs`로 볼륨 설정을 저장/로드 |
| `MapManager.cs` | `ObstaclePool`을 참조하여 `JSON` 파일(`ObstaclePattern.cs`)에 정의된 장애물 패턴을 맵에 스폰. `topPatternCount` 등을 이용해 연속적인 패턴이 나오지 않도록 관리 |

### UI (사용자 인터페이스)

| 스크립트 | 설명 |
| :--- | :--- |
| `BaseUI.cs` | 모든 UI 패널의 기반이 되는 `abstract` 클래스. `UIManager`가 호출하는 `SetActive(UIState state)` 공통 로직을 포함하며, 자식 클래스는 `GetUIState()`를 반드시 구현해야 함 |
| `TitleUI.cs` | 타이틀 씬의 UI. (게임 시작, 캐릭터 씬 이동, 설정, 종료 버튼) |
| `CharacterUI.cs` | 펫 선택 씬의 UI. `PlayerPrefs`를 사용해 선택한 펫의 ID(`SELECTED_PET_ID_KEY`)를 기기에 저장 |
| `PlayUI.cs` | 인게임 플레이 중의 UI. (HP 슬라이더, 점수, 일시정지/점프/슬라이드 버튼) `EventManager`를 구독하여 `UpdateHPSlider`, `UpdateScoreText`가 호출됨 |
| `PauseUI.cs` | 일시정지 팝업 UI. (계속하기, 재시작, 타이틀로) `GameManager.Instance.ResumeGame()`을 호출 |
| `GameOverUI.cs` | 게임오버 팝업 UI. (최종 점수 표시, 재시작, 타이틀로) `UpdateScoreText`로 최종 점수를 표시 |
| `SettingUI.cs` | 설정 팝업 UI. BGM/SFX 슬라이더를 통해 `SoundManager.instance.SetBgmVolume`을 직접 호출하여 볼륨을 실시간 제어. |
| `HoldableButton.cs` | `IPointerDownHandler`, `IPointerUpHandler`를 구현한 커스텀 버튼. 슬라이드 버튼처럼 '누르고 떼는' 동작을 감지하기 위해 `onPress`, `onRelease` 이벤트를 제공 |

### Pet (펫 시스템)

| 스크립트 | 설명 |
| :--- | :--- |
| `PetData.cs` | `ScriptableObject`를 사용한 펫 데이터 컨테이너. 펫의 고유 ID, 이름, 스프라이트 정보를 담고 있으며, `CreateAssetMenu`를 통해 에디터에서 쉽게 생성/관리할 수 있다. |
| `PetMoving.cs` | 인게임에서 펫의 움직임을 구현. `PlayerPrefs`에서 선택한 펫 ID를 로드하여 `SpriteRenderer`에 적용. `Update`에서 `Mathf.Sin`을 사용해 위아래로 부드럽게 떠다니는(Bobbing) 효과를 준다. |

### Item (아이템)

| 스크립트 | 설명 |
| :--- | :--- |
| `Items.cs` | 개별 아이템(HP, Score)의 로직. `OnTriggerEnter2D`로 플레이어와 충돌 시 `GameManager`의 HP/점수를 변경하고, `ItemPool`에 자신을 반환 |
| `ItemPool.cs` | 아이템(HP, Score) 프리팹을 관리하는 오브젝트 풀. `Queue`를 사용해 비활성화된 아이템을 보관하고 재활용 |
| `ItemSpawner.cs` | 플레이어 위치를 기준으로 아이템을 스폰. `Physics2D.OverlapCircle`로 장애물과 겹치지 않게 위치를 보정하는 로직을 포함 |

### Obstacle (장애물)

| 스크립트 | 설명 |
| :--- | :--- |
| `Obstacles.cs` | 개별 장애물의 로직. `OnTriggerEnter2D`로 플레이어와 충돌 시 `Player.TakeHit()`를 호출하여 데미지를 준다. |
| `ObstaclePool.cs` | 장애물(Top, Bottom, Double) 프리팹을 관리하는 오브젝트 풀. `InitializePool`에서 `ShuffleList`로 리스트를 섞어 다양한 장애물이 나오도록 구현. |
| `ObstaclePattern.cs` | `MapManager`가 읽어올 JSON 데이터의 C# 클래스 구조체. `[Serializable]` 어트리뷰트를 사용 |
| `ObstacleReturner.cs` | 화면 왼쪽 밖으로 나간 장애물/아이템을 감지하여 각 `Pool`에 반환하는 역할 |

### 플레이어 및 환경

| 스크립트 | 설명 |
| :--- | :--- |
| `Player.cs` | 플레이어의 모든 로직을 담당. `FixedUpdate`에서 전진 이동 및 착지 판정. `Update`에서 키보드/UI 입력을 받아 `TryJump`, `BeginSlide` 등을 호출. `TakeHit`로 피격 시 `InvincibleCoroutine` (무적 코루틴)을 실행. |
| `FollowCamera.cs` | `target`(플레이어)을 `offsetX`만큼 간격을 두고 따라가는 메인 카메라 로직. |
| `BgLooper.cs` | 2개의 배경 타일을 `parallax` 값(원근감)에 맞춰 이동시키고, 화면 밖으로 벗어나면 반대편 뒤로 재배치하여 무한 배경 스크롤을 구현. |

---
