# Trabajadores del Mundo Unidos

Workers of the world unite!

## Meters

* Class Consciousness (Strengthens all workers)

## Gameplay

There are bosses, class conscious workers (player characters), and workers (non player characters). Probably 1 boss, 4 class conscious workers, or some adaptive balancing mechanism to allow different numbers.

The bosses extract surplus value from all workers and try to achieve a target extraction rate. They also attempt to decrease the class consciousness meter to reduce the effectiveness of all workers.

Class conscious workers attempt to execute goals like unionization to reduce the bosses' power and eventually to overthrow them.

The bosses have an [isometric camera perspective](../Design/CameraPerspectives.md#Isometric) of the level and don't manage an actual visible GameObject/Actor in the scene.

Class conscious workers each manage a single GameObject/Actor and have a TBD [camera perspective](../Design/CameraPerspectives.md).

### Minority Mechanisms

#### Increase Capital Extraction

The bosses can spend their extracted value to increase the rate at which they extract value from one of the workers.

#### Break work stoppage

Increases class consciousness by a function of itself (y = CC^1.5 or something like that).

#### Anti-union training

Decreases class consciousness, all workers stop producing during training and receive anti-union messaging on screen.

#### Divide and Conquer

Divides the NPC workers against each other on race, gender, etc. to discourage cooperation, reducing class consciousness.

### Majority Mechanisms

#### Work Stoppage

Class conscious workers can perform a work stoppage in order to starve the bosses. NPC workers stop producing during the work stoppage.

#### Rewards

Looting rewards can be done by any of the class conscious workers, regardless of who unlocked the reward (killing mobs, opening chests, or whatever other mechanisms we want).

Class conscious workers can share the rewards that way to maximize their rate of purchasing rewards.

#### Propagandize

Spread pro-union messaging to increase class consciousness.

#### Unionize

Unionization significantly increases the NPC workers effectiveness in worker revolution

#### Worker revolution

All the workers seize the means of production and overthrow the bosses.

## Theming

Bosses -> XP Trolls
Workers -> Adventurers

Capital -> XP
Value -> XP

Class Consciousness -> ?
Increase Capital Extraction -> Tithe
Union -> Fellowship
Work Stoppage -> ?
Worker Revolution -> Troll Overthrow
