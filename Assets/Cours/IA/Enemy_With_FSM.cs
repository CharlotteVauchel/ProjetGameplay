using UnityEngine;
using UnityEngine.AI;

public class Enemy_With_FSM : MonoBehaviour
{
    public enum EnemyStates { Idling, Searching, Shooting };

    public class EnemyIdling : State<EnemyStates>
    {
        private Enemy_With_FSM enemy;

        private float secondsToIdle;
        private float enterTime;

        public EnemyIdling(Enemy_With_FSM _enemy) : base(EnemyStates.Idling)
        {
            enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();

            //TODO_Exercice_2: Resetez: la variable Speed de l'animator,
            enemy.animator.SetFloat("Speed", 0);
            // la variable enterTime
            enterTime = Time.time;
            // et définissez une durée "secondsToIdle" à l'aide de Random.Range
            secondsToIdle = Random.Range(0, 20);

        }

        public override void Update()
        {
            base.Update();
            //TODO_Exercice_2: vérifiez qu'on a attendu assez longtemps, si c'est le cas, passez en état Searching
            if (Time.time - enterTime > secondsToIdle)
            {
                enemy.fsm.SetCurrentState(EnemyStates.Searching);
            }
        }
    }

    public class EnemySearching : State<EnemyStates>
    {
        private Enemy_With_FSM enemy;
        private Vector3 lastKnownPlayerPosition;
        private Vector3 previousEnemyPosition;

        public EnemySearching(Enemy_With_FSM _enemy) : base(EnemyStates.Searching)
        {
            enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();
            //TODO_Exercice_2: initialisez les deux positions
            lastKnownPlayerPosition = enemy.player.transform.position;
            previousEnemyPosition = enemy.transform.position;

        }

        public override void Update()
        {
            base.Update();

            //TODO_Exercice_2: dites à votre agent d'aller vers la dernière position connue du joueur, mettez à jour la variable de l'animator (Speed)
            enemy.agent.SetDestination(lastKnownPlayerPosition);
            enemy.animator.SetFloat("Speed", enemy.agent.velocity.magnitude/enemy.agent.speed);


            // TODO_Exercice_2: nous sommes proche de la dernière position connue du joueur
            if (Vector3.Distance(enemy.transform.position, lastKnownPlayerPosition) < 5)
            {
                Vector3 directionTowardsPlayer = (previousEnemyPosition - enemy.player.transform.position).normalized;
                enemy.animator.SetFloat("Speed", 0);

                // si le joueur est proche et si il n'y à pas d'obstacles avec le joueur et passez en Shooting,
                //Tire d'un point A à un poitn B -> calcul de collision + on créé un hit si jamais il y a collision 
                if (Physics.Raycast(enemy.transform.position, directionTowardsPlayer, out RaycastHit hit))
                {
                    //La collision peut être un player ou un baril ou autre chose on vérifie que le collider est un gameObject et pour vérifier que c'est un joueur
                    //on ajoute la vérification d'un component unique au joueur (ici un playerController)
                    if(hit.collider.gameObject.GetComponent<PlayerController>() != null)
                    {
                        enemy.fsm.SetCurrentState(EnemyStates.Shooting);
                    }
                }
            }
            // sinon retournez en Idling
            else
            {
                enemy.fsm.SetCurrentState(EnemyStates.Idling);
            }

            previousEnemyPosition = enemy.transform.position;
        }

    }

    public class EnemyShooting : State<EnemyStates>
    {
        private Enemy_With_FSM enemy;
        public EnemyShooting(Enemy_With_FSM _enemy) : base(EnemyStates.Shooting)
        {
            enemy = _enemy;
        }

        public override void Enter()
        {
            base.Enter();

            //TODO_Exercice_2: remettez à 0 la variable Speed de l'animator
            enemy.animator.SetFloat("Speed", 0);

            Vector3 directionTowardsPlayer = (enemy.transform.position - enemy.player.transform.position).normalized;

            //TODO_Exercice_2: avec Physics.Raycast, tentez de toucher le joueur
            if(Physics.Raycast(enemy.transform.position, directionTowardsPlayer, out RaycastHit hit))
            {
                if(hit.collider.gameObject.GetComponent <PlayerController>() != null)
                {
                    Debug.Log("Player touched");
                }
            }

            enemy.fsm.SetCurrentState(EnemyStates.Idling);
        }
    }


    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;

    private PlayerController player;
    private FiniteStateMachine<EnemyStates> fsm = new FiniteStateMachine<EnemyStates>();

    void Start()
    {
        player = PlayerController.instance;

        fsm.Add(new EnemyIdling(this));
        fsm.Add(new EnemySearching(this));
        fsm.Add(new EnemyShooting(this));

        fsm.SetCurrentState(EnemyStates.Idling);
    }

    private void Update()
    {
        fsm.Update();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

}
